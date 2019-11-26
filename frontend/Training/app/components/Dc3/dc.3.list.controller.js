'use strict';

/**
 * @ngdoc function
 * @name Training.controller:Dc3ListController
 * @description
 * # Dc3ListController
 * Controller of the Training
 */
angular.module('Training').controller('Dc3ListController',
    function($scope,
        listController,
        Dc3Service,
        $mdDialog,
        $rootScope,
        MatrixService,
        LevelService,
        CertificationService,
        TrainingService,
        FormatDc3Service,
        EmployeeService,
        ShiftService) {

        // var listCtrl = new listController({
        //     scope: $scope,
        //     entityName: 'Dc3',
        //     baseService: Dc3Service,
        //     afterCreate: function(oInstance, oEvent) {
        //         $mdDialog.show({
        //             title: 'Dc3',
        //             contentElement: '#modal-Dc3',
        //             parent: angular.element(document.body),
        //             clickOutsideToClose: true,
        //             multiple: true,
        //             fullscreen: true,
        //             targetEvent: oEvent,
        //             onRemoving: function(element, removePromise) {
        //                 listCtrl.load();
        //             }
        //         });

        //         $rootScope.$broadcast('load-modal-Dc3', oInstance);
        //     },
        //     afterLoad: function() {

        //     },
        //     onOpenItem: function(oEntity, oEvent) {
        //         $mdDialog.show({
        //             title: 'Dc3',
        //             contentElement: '#modal-Dc3',
        //             parent: angular.element(document.body),
        //             clickOutsideToClose: true,
        //             multiple: true,
        //             fullscreen: true,
        //             targetEvent: oEvent,
        //             onRemoving: function(element, removePromise) {
        //                 listCtrl.load();
        //             }
        //         });

        //         $rootScope.$broadcast('load-modal-Dc3', oEntity);
        //     },
        //     filters: []
        // });

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

        $scope.$on('load_Dc3', function(scope) {
            listCtrl.load();
        });

        listCtrl.load();

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
            debugger;
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

        $scope.openDC3Format = function(oEvent, oCertification, oEmployee) {

            /* FormatDc3Service.getSingleWhere('TrainingScoreKey', oTrainingScore.id).then(function(data) {
                if (data && data.id > 0) {
                    $mdDialog.show({
                        title: 'FormatDc3',
                        contentElement: '#modal-FormatDc3',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent
                    });
                    $rootScope.$broadcast('load-modal-FormatDc3', data);
                } else { */
            var confirm = $mdDialog.confirm()
                .title('DC3 Format does not exist yet, do you want to create it now?')
                .textContent('Please confirm DC3 Format creation.')
                // .placeholder('')
                // .ariaLabel('')
                // .initialValue('')
                .targetEvent(oEvent)
                .multiple(true)
                // .required(false)
                .ok('Yes')
                .cancel('No');

            $mdDialog.show(confirm).then(function(result) {
                $mdDialog.show({
                    title: 'FormatDc3',
                    contentElement: '#modal-FormatDc3',
                    parent: angular.element(document.body),
                    clickOutsideToClose: true,
                    multiple: true,
                    fullscreen: true,
                    targetEvent: oEvent
                });

                oCertification.forCurrentEmployee.Employee = oEmployee;
                oCertification.forCurrentEmployee.Training = oCertification.TrainingInfo;

                FormatDc3Service.createEntity({
                    EmployeeKey: oEmployee.id,
                    CertificationKey: oCertification.id,
                    TrainingScoreKey: oCertification.forCurrentEmployee.TrainingScoreKey
                }).then(function(oInstance) {
                    oInstance.Employee = oEmployee;
                    oInstance.Certification = oCertification;

                    $rootScope.$broadcast('load-modal-FormatDc3', oInstance, oCertification.forCurrentEmployee);
                });
            });
            // }
            // });
        };


    });
