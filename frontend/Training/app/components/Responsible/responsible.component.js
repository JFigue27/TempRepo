'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Responsible
 * @description
 * # Responsible
 */
angular.module('Training').directive('responsible', function() {
    return {
        templateUrl: 'components/Responsible/responsible.html',
        restrict: 'E',
        scope: {
            infotrack: '=',
            title: '@'
        },
        controller: function($scope, formController, userService, $mdDialog, trackService) {

            var ctrl = this;

            formController.call(this, {
                scope: $scope,
                entityName: 'User',
                baseService: userService,
                afterCreate: function(oEntity) {

                },
                afterLoad: function(oEntity) {

                }
            });

            $scope.baseEntity = {};

            // ctrl.save().then(function() {
            //     $mdDialog.hide();
            // });

            function refresh() {
                $scope.isLoading = true;
                userService.loadEntity($scope.infotrack.User_AssignedToKey).then(function(data) {
                    if (data == null) {
                        $scope.baseEntity = {};
                    } else {
                        ctrl.load(data);
                    }
                });
            }

            $scope.$watch(function() {
                if ($scope.infotrack) {
                    return $scope.infotrack.User_AssignedToKey;
                }
                return null;
            }, function(oNewValue) {
                if (oNewValue > 0) {
                    refresh();
                }
            });

            $scope.changeResponsible = function() {
                $mdDialog.show({
                    title: '',
                    contentElement: '#modal-responsible',
                    parent: angular.element(document.body),
                    clickOutsideToClose: true,
                    multiple: true,
                    fullscreen: true,
                    onRemoving: function(element, removePromise) {
                        listCtrl.load();
                    }
                });
                $scope.$broadcast('load_responsible_list', $scope.infotrack.User_AssignedToKey);
            };

            this.changeUser = function(oNewUserId) {
                return trackService.assignResponsible($scope.infotrack.id, oNewUserId).then(function() {
                    $scope.infotrack.User_AssignedToKey = oNewUserId;
                });
            };

        }
    };
});
