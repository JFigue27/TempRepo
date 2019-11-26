'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:TrainingScore
 * @description
 * # TrainingScore
 */
angular.module('Training').directive('trainingScoreList', function() {
    return {
        templateUrl: 'components/TrainingScore/training.score.list.html',
        restrict: 'E',
        controller: function($scope, listController, $mdDialog, $rootScope, TrainingScoreService, EmployeeService) {

            var listCtrl = new listController({
                scope: $scope,
                entityName: 'TrainingScore',
                baseService: TrainingScoreService,
                paginate: false,
                afterCreate: function(oInstance, oEvent) {
                    $mdDialog.show({
                        title: 'TrainingScore',
                        contentElement: '#modal-TrainingScore',
                        parent: angular.element(document.body),
                        clickOutsideToClose: false,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent
                    });

                    $rootScope.$broadcast('load-modal-TrainingScore', oInstance);
                },
                afterLoad: function() {
                    // console.log($scope.baseList)
                    // if ($scope.baseEntity) {
                    // $scope.handleDynamicRows($scope.baseList);
                    // }
                },
                onOpenItem: function(oEntity, oEvent) {
                    $mdDialog.show({
                        title: 'TrainingScore',
                        contentElement: '#modal-TrainingScore',
                        parent: angular.element(document.body),
                        clickOutsideToClose: false,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent
                    });

                    $rootScope.$broadcast('load-modal-TrainingScore', oEntity);
                },
                filters: []
            });

            $scope.$on('load_TrainingScore', function(scope) {
                listCtrl.load('TrainingKey=' + $scope.baseEntity.TrainingKey);
            });

            // listCtrl.load();


            $scope.handleDynamicRows = function(arrRows) {
                if (arrRows.length > 0) {
                    var atLeastOneCellFilled = false;
                    var lastRow = arrRows[arrRows.length - 1]
                    for (var prop in lastRow) {
                        if (lastRow.hasOwnProperty(prop)) {
                            if (prop == '$$hashKey' || prop == 'id') {
                                continue;
                            }
                            if (lastRow[prop] !== null && lastRow[prop] !== undefined && (lastRow[prop] > 0 || lastRow[prop].length > 0)) {
                                atLeastOneCellFilled = true;
                                break;
                            }
                        }
                    }
                    if (!atLeastOneCellFilled) {
                        return;
                    }
                }

                arrRows.push({});
            };

            $scope.removeEmployee = function(fromArray, index) {
                if (fromArray[index].id > 0) {
                    fromArray[index].EF_State = 3;
                } else {
                    fromArray.splice(index, 1);
                }
                // $scope.handleDynamicRows(fromArray);
            };

            $scope.undoRemoveEmployee = function(oScore) {
                oScore.EF_State = 0;
            };

            // EmployeeService.getFilteredPage(0, 1, "?JobPositionKey=1").then(function(oResponse) {
            //     $scope.theSupervisors = oResponse.Result;
            // });

            $scope.queryEmployees = function(sQuery) {
                return EmployeeService
                    .getFilteredPage(30, 1, '?filterGeneral=' + sQuery)
                    .then(function(response) {
                        return response.Result;
                    });
            };

            $scope.newEmployee = function(sValue) {
                if (sValue) {
                    EmployeeService.createEntity()
                        .then(function(oInstance) {
                            oInstance.Value = sValue;
                            return oInstance;
                        })
                        .then(EmployeeService.save)
                        .then(function(oEntity) {
                            $scope.searchEmployee = '';
                            alertify.success('Created Successfully.');
                            $timeout(function() {
                                $scope.searchEmployee = sValue;
                            });
                        })
                        .catch(function() {
                            alertify.error('An error has occurried');
                        });
                }
            };

            $scope.addEmployee = function(oEmployee) {

                if (oEmployee) {

                    var oFind = $scope.baseList.find(function(obj) {
                        return obj.EmployeeKey == oEmployee.id;
                    });

                    if (oFind) {
                        alertify.message('Already in list.');
                        return;
                    }

                    $scope.baseList.unshift({
                        Employee: oEmployee,
                        EmployeeKey: oEmployee.id,
                        Score: "100"
                    });
                }
            };
        }
    };
}).directive('trainingsByEmployee', function() {
    return {
        templateUrl: 'components/TrainingScore/training.score.list.short.html',
        restrict: 'E',
        scope: {},
        controller: 'TrainingScoreListController'
    }
});
