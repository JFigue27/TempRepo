'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:UserList
 * @description
 * # UserList
 */
angular.module('Training').directive('userList', function() {
    return {
        templateUrl: 'components/User/user.list.html',
        restrict: 'E',
        controller: function($scope, listController, userService, $mdDialog, $rootScope) {

            var listCtrl = new listController({
                scope: $scope,
                entityName: 'User',
                baseService: userService,
                afterCreate: function(oInstance, oEvent) {
                    $mdDialog.show({
                        title: 'User',
                        contentElement: '#modal-user',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-user', oInstance);
                },
                afterLoad: function() {

                },
                onOpenItem: function(oEntity, oEvent) {
                    $mdDialog.show({
                        title: 'User',
                        contentElement: '#modal-user',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-user', oEntity);
                },
                filters: []
            });

            listCtrl.load();
        }
    };
});
