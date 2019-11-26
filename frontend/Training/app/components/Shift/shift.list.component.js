'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Shift
 * @description
 * # Shift
 */
angular.module('Training').directive('shiftList', function() {
    return {
        templateUrl: 'components/Shift/shift.list.html',
        restrict: 'E',
        controller: function($scope, listController, $mdDialog, $rootScope, ShiftService) {

            var listCtrl = new listController({
                scope: $scope,
                entityName: 'Shift',
                baseService: ShiftService,
                afterCreate: function(oInstance, oEvent) {
                    $mdDialog.show({
                        title: 'Shift',
                        contentElement: '#modal-Shift',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-Shift', oInstance);
                },
                afterLoad: function() {

                },
                onOpenItem: function(oEntity, oEvent) {
                    $mdDialog.show({
                        title: 'Shift',
                        contentElement: '#modal-Shift',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-Shift', oEntity);
                },
                filters: []
            });

            $scope.$on('load_Shift', function(scope) {
                listCtrl.load();
            });

            listCtrl.load();
        }
    };
});
