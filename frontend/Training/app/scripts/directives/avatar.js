'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:avatar
 * @description
 * # avatar
 */
angular.module('Training').directive('avatar', function(FileUploader, appConfig, $http, $timeout, $q, utilsService) {
    return {
        restrict: 'E',
        scope: {
            ownerEntity: '=',
            printmode: '=',
            readOnly: '=',
            customPropertyBind: '@',
            customFolderBind: '@',
            whenChange: '&'
        },
        template: `<div nv-file-drop nv-file-over uploader="uploader" class="avatar" over-class="UploaderOverClass" ng-click="$event.stopPropagation();" ng-dblclick="addAttachment();$event.stopPropagation();">
                    <input type="file" nv-file-select uploader="uploader" style="display:none;" />
                    <div>
                        <img ng-src="{{'data:image/png;base64,' + ownerEntity[avatarList][0].ImageBase64}}" ng-style="{display: ownerEntity[avatarList][0].ImageBase64 ? 'block': 'none'}" class="img-circle" style="width:84%;" />
                        <md-icon ng-style="{display: ownerEntity[avatarList][0].ImageBase64 ? 'none': 'block'}" class="img-circle" style="font-size: 60px;padding-left:5px;width:70px;height:70px;margin:auto;border: solid 1px lightgray;">person</md-icon>
                    </div>
                </div>`,
        compile: function compile(tElement, tAttrs, transclude) {
            return {
                pre: function preLink(scope, iElement, iAttrs, controller) {
                    scope.uploader = new FileUploader();
                    scope.avatarList = scope.customPropertyBind || 'AvatarList';
                    scope.avatarFolder = scope.customFolderBind || 'AvatarFolder';

                    //When user selects a file from browser:
                    scope.uploader.onAfterAddingFile = function(fileItem) {
                        if (!scope.ownerEntity[scope.avatarList]) {
                            scope.ownerEntity[scope.avatarList] = [];
                        }
                        scope.ownerEntity[scope.avatarList].push({
                            FileName: fileItem.file.name,
                            Directory: (scope.ownerEntity[scope.avatarFolder] || ''),
                            isForUpload: true
                        });
                        scope.whenChange({
                            oEntity: scope.ownerEntity
                        });
                    };
                    scope.uploader.onWhenAddingFileFailed = function(item, filter, options) {
                        // console.debug(item);
                        // console.debug(filter);
                        // console.debug(options);
                    };
                    //A single file was uploaded successfully.
                    scope.uploader.onSuccessItem = function(item, response, status, headers) {
                        var backendResponse = response;
                        if (!backendResponse.ErrorThrown) {
                            scope.ownerEntity[scope.avatarFolder] = backendResponse.ResponseDescription;
                            scope.ownerEntity[scope.avatarList] = backendResponse.Result;
                            var theAttachment = scope.getAttachment(item.file.name);
                            theAttachment.isForUpload = false;
                        } else {
                            scope.ErrorThrown = true;
                            alertify.alert(backendResponse.ResponseDescription).set('modal', true);
                            console.debug(response);
                        }
                    };
                    scope.uploader.onErrorItem = function(item, response, status, headers) {
                        console.debug(item);
                        console.debug(response);
                        console.debug(status);
                    };

                    //All files where uploaded to backend successfully:
                    scope.uploader.onCompleteAll = function() {
                        if (!scope.ErrorThrown) {
                            scope.uploading.resolve();
                        } else {
                            scope.uploading.reject();
                        }
                    };
                    scope.uploader.onBeforeUploadItem = function(item) {
                        item.url = appConfig.API_URL + 'avatar?targetFolder=' + (scope.ownerEntity[scope.avatarFolder] || '')
                    };
                },
                post: function(scope, iElement, iAttrs) {
                    scope.baseURL = appConfig.API_URL;
                    scope.$watch(function() {
                        return scope.ownerEntity;
                    }, function() {
                        if (scope.ownerEntity) {
                            var apiName = 'api_avatar';
                            if (scope.customPropertyBind) {
                                apiName = 'api_' + scope.customPropertyBind;
                            }
                            scope.ownerEntity[apiName] = {};
                            scope.ownerEntity[apiName].uploadAvatar = function() {
                                scope.uploading = $q.defer();
                                scope.ErrorThrown = false;
                                if (scope.uploader.getNotUploadedItems().length > 0) {
                                    scope.uploader.uploadAll();
                                } else {
                                    scope.uploading.resolve();
                                }
                                return scope.uploading.promise;
                            };
                            scope.ownerEntity[apiName].clearQueue = function() {
                                scope.uploader.clearQueue();
                            };
                        }
                    }, false);
                    scope.theElement = iElement;

                    //User clicks "X" to one of the files.
                    scope.removeAttachment = function(attachment, index) {

                        if (attachment.isForUpload) {
                            scope.uploader.removeFromQueue(scope.getItem(attachment.FileName));
                            scope.ownerEntity[scope.avatarList].splice(index, 1);
                        } else {
                            scope.ownerEntity[scope.avatarList][index].ToDelete = true;
                            // alertify.confirm(
                            //     'This action cannot be undo, do you want to continue?',
                            //     function() {
                            //         scope.$apply(function() {
                            //             $http.get(appConfig.API_URL + 'attachment_delete?directory=' + attachment.Directory + '&fileName=' + attachment.FileName + '&attachmentKind=' + scope.kind).then(function(data) {
                            //                 var backendResponse = data;
                            //                 if (!backendResponse.ErrorThrown) {
                            //                     scope.ownerEntity[scope.avatarList].splice(index, 1);
                            //                     $timeout(function() {
                            //                         alertify.success('File deleted successfully.');
                            //                     }, 100);
                            //                 } else {
                            //                     alertify.alert('An error has occurried, see console for more details.').set('modal', true);
                            //                     console.debug(response);
                            //                 }
                            //                 scope.afterDelete({
                            //                     oEntity: scope.ownerEntity
                            //                 });
                            //             }, function(data) {
                            //                 alertify.alert('An error has occurried, see console for more details.').set('modal', true);
                            //                 console.debug(data);
                            //             });
                            //         });
                            //     });
                        }
                    };
                    scope.cancelRemove = function(index) {
                        scope.ownerEntity[scope.avatarList][index].ToDelete = false;
                    };
                    scope.getItem = function(sName) {
                        for (var i = 0; i < scope.uploader.queue.length; i++) {
                            if (scope.uploader.queue[i].file.name == sName) {
                                return scope.uploader.queue[i];
                            }
                        }
                        return null;
                    };
                    //When click "Add File":
                    scope.addAttachment = function() {
                        if (!scope.readOnly) {
                            $timeout(function() {
                                scope.theElement.find('input[type="file"]').click();
                            });
                        }
                    };
                    scope.getAttachment = function(sName) {
                        for (var i = 0; i < scope.ownerEntity[scope.avatarList].length; i++) {
                            if (scope.ownerEntity[scope.avatarList][i].FileName == sName) {
                                return scope.ownerEntity[scope.avatarList][i];
                            }
                        }
                        return null;
                    };

                    scope.getEncodedFileName = function(attachment) {
                        return encodeURIComponent(attachment.FileName);
                    };

                    scope.getImageBase64 = function(avatar) {
                        if (avatar) {
                            return utilsService.toImageBase64(avatar.ImageBase64);
                        }
                        return null;
                    }
                }
            };
        }
    };
});
