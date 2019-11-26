'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:EmployeeCard
 * @description
 * # EmployeeCard
 */
angular.module('Training').directive('employeeCardList', function() {
    return {
        templateUrl: 'components/EmployeeCard/employee.card.list.html',
        restrict: 'E',
        controller: function($scope, listController, $mdDialog, $rootScope, EmployeeService) {

            var listCtrl = new listController({
                scope: $scope,
                entityName: 'Employee',
                baseService: EmployeeService,
                afterCreate: function(oInstance, oEvent) {
                    $mdDialog.show({
                        title: 'EmployeeCard',
                        contentElement: '#modal-EmployeeCard',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-EmployeeCard', oInstance);
                },
                afterLoad: function() {

                },
                onOpenItem: function(oEntity, oEvent) {
                    $mdDialog.show({
                        title: 'EmployeeCard',
                        contentElement: '#modal-EmployeeCard',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-EmployeeCard', oEntity);
                },
                filters: []
            });

            $scope.$on('load_EmployeeCard', function(scope) {
                listCtrl.load();
            });

            listCtrl.load();
        }
    };
});
