'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Employee
 * @description
 * # Employee
 */
angular.module('Training').directive('employeeList', function() {
    return {
        templateUrl: 'components/Employee/employee.list.html',
        restrict: 'E',
        controller: function($scope, listController, $mdDialog, $rootScope, EmployeeService) {

            var listCtrl = new listController({
                scope: $scope,
                entityName: 'Employee',
                baseService: EmployeeService,
                afterCreate: function(oInstance, oEvent) {
                    $mdDialog.show({
                        title: 'Employee',
                        contentElement: '#modal-Employee',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-Employee', oInstance);
                },
                afterLoad: function() {

                },
                onOpenItem: function(oEntity, oEvent) {
                    $mdDialog.show({
                        title: 'Employee',
                        contentElement: '#modal-Employee',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-Employee', oEntity);
                },
                filters: []
            });

            $scope.$on('load_Employee', function(scope) {
                listCtrl.load();
            });

            listCtrl.load();

            $scope.openEmployeeCard = function(oEmployee, oEvent) {
                $mdDialog.show({
                    title: 'EmployeeCard',
                    contentElement: '#modal-EmployeeCard',
                    parent: angular.element(document.body),
                    clickOutsideToClose: true,
                    multiple: true,
                    fullscreen: true,
                    targetEvent: oEvent
                });

                $rootScope.$broadcast('load-modal-EmployeeCard', oEmployee.id);
            };

            $scope.openCertifications = function(oEmployee, oEvent) {
                $mdDialog.show({
                    title: 'Certifications',
                    contentElement: '#modal-Certifications',
                    parent: angular.element(document.body),
                    clickOutsideToClose: true,
                    multiple: true,
                    fullscreen: true,
                    targetEvent: oEvent
                });

                $rootScope.$broadcast('load-TrainingsByEmployee', oEmployee);
            };


        }
    };
});
