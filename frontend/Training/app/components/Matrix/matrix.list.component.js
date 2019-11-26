'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Matrix
 * @description
 * # Matrix
 */
angular.module('Training').directive('matrixList', function() {
    return {
        templateUrl: 'components/Matrix/matrix.list.html',
        restrict: 'E',
        controller: function($scope,
            listController,
            $mdDialog,
            $rootScope,
            MatrixService,
            LevelService,
            CertificationService,
            TrainingService,
            ShiftService) {

            var listCtrl = new listController({
                scope: $scope,
                entityName: 'Matrix',
                baseService: MatrixService,
                afterCreate: function(oInstance, oEvent) {
                    $mdDialog.show({
                        title: 'Matrix',
                        contentElement: '#modal-Matrix',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-Matrix', oInstance);
                },
                afterLoad: function() {
                    // refreshProductionLines();
                    refreshCertifications();
                    console.log($scope.baseList)
                },
                onOpenItem: function(oEntity, oEvent) {
                    $mdDialog.show({
                        title: 'Matrix',
                        contentElement: '#modal-Matrix',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-Matrix', oEntity);
                },
                filters: []
            });

            $scope.$on('load_Matrix', function(scope) {
                refresh();
            });


            function refresh() {
                listCtrl.load();

            }

            LevelService
                .loadEntities(true)
                .then(function(response) {
                    $scope.theAreas = response.Result;
                });

            ShiftService
                .loadEntities(true)
                .then(function(response) {
                    $scope.theShifts = response.Result;
                });


            var trainings = [];

            var refreshCertifications = function() {
                $scope.theCertifications = [];

                //Level1Key required to get columns
                if ($scope.filterOptions && $scope.filterOptions.Level1Key) {

                    var area = LevelService.getById($scope.filterOptions.Level1Key);
                    if (area) {

                        $scope.theCertifications = area.Certifications;

                        if ($scope.filterOptions.AppliesToDC3) {
                            $scope.theCertifications = area.Certifications.filter(function(oItem) {
                                return oItem.AppliesToDC3 == true;
                            });
                        }


                        TrainingService.GetActiveTrainings($scope.filterOptions.Level1Key)

                            // TrainingService.getFilteredPage(0, 1, '?Level1Key=' + $scope.filterOptions.Level1Key)
                            .then(function(oResponse) {

                                trainings = oResponse;

                                $scope.baseList.forEach(function(oEmployee) {
                                    oEmployee.CertificationsByEmployee = angular.copy($scope.theCertifications);

                                    oEmployee.CertificationsByEmployee.forEach(function(oCertification) {
                                        var trainingsForCertification = trainings.filter(function(oTraining) {
                                            return oTraining.CertificationKey == oCertification.id;
                                        });

                                        if (trainingsForCertification && trainingsForCertification.length > 0) {

                                            //certification is taken when score > 0
                                            trainingsForCertification.forEach(function(oTrain) {
                                                var certificationTaken = oTrain.TrainingScores.find(function(oScore) {
                                                    return oScore.EmployeeKey == oEmployee.id;
                                                });

                                                if (certificationTaken) {
                                                    oCertification.TrainingInfo = oTrain;
                                                    oCertification.forCurrentEmployee = certificationTaken;
                                                }
                                            });

                                        }
                                    });
                                });

                            });
                    }

                }
            };

            // function refreshProductionLines() {
            //     return LevelService
            //         .getFilteredPage(0, 1, '?AreaKey=' + $scope.filterOptions.AreaKey)
            //         .then(function(response) {
            //             LevelService.setRawAll(response.Result);
            //             $scope.theProductionLines = response.Result;
            //             refreshCertifications();
            //         });
            // }

            // $scope.onAreaChange = function() {
            //     $scope.filterOptions.Level1Key = null;
            //     refreshProductionLines();
            // };

            // $scope.onShiftChange = function() {
            //     // alert($scope.filterOptions.ShiftKey);
            // };


            $scope.openQuickCertification = function(oEvent, oCertification, oItem) {
                $mdDialog.show({
                    contentElement: '#modal-QuickCertification',
                    parent: angular.element(document.body),
                    clickOutsideToClose: true,
                    multiple: true,
                    fullscreen: true,
                    targetEvent: oEvent
                }).then(function() {
                    refresh();
                });

                $rootScope.$broadcast('initializeQuickTraining', oCertification, oItem);
            };

            $scope.openEmployeeCard = function(oEntity, oEvent) {
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

                $rootScope.$broadcast('load-modal-EmployeeCard', oEntity.id);
            };

            refresh();

        }
    };
});
