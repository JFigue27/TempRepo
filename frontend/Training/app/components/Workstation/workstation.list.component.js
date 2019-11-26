'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Workstation
 * @description
 * # Workstation
 */
angular.module('Training').directive('workstationList', function() {
    return {
        templateUrl: 'components/Workstation/workstation.list.html',
        restrict: 'E',
        controller: function($scope, listController, $mdDialog, $rootScope, WorkstationService) {

            var listCtrl = new listController({
                scope: $scope,
                entityName: 'Workstation',
                baseService: WorkstationService,
                afterCreate: function(oInstance, oEvent) {
                    $mdDialog.show({
                        title: 'Workstation',
                        contentElement: '#modal-Workstation',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-Workstation', oInstance);
                },
                afterLoad: function() {

                },
                onOpenItem: function(oEntity, oEvent) {
                    $mdDialog.show({
                        title: 'Workstation',
                        contentElement: '#modal-Workstation',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-Workstation', oEntity);
                },
                filters: []
            });

            $scope.$on('load_Workstation', function(scope) {
                listCtrl.load();
            });

            listCtrl.load();
        }
    };
});
