'use strict';

/**
 * @ngdoc function
 * @name Training.controller:TrainingScoreListController
 * @description
 * # TrainingScoreListController
 * Controller of the Training
 */
angular.module('Training').controller('TrainingScoreListController',
    function($scope,
        listController,
        TrainingScoreService,
        $mdDialog,
        $rootScope,
        FormatDc3Service,
        $location) {

        var ctrl = new listController({
            scope: $scope,
            entityName: 'TrainingScore',
            baseService: TrainingScoreService,
            afterCreate: function(oInstance) {

            },
            afterLoad: function() {

            },
            onOpenItem: function(oItem) {

            },
            filters: {

            }
        });

        $scope.$on('load-TrainingsByEmployee', function(scope, oEmployee) {
            $scope.oEmployee = oEmployee;
            refresh();
        });
        $scope.onIluoChange = function(item){
            console.log(item);
            TrainingScoreService.save(item).then(function(){
                alertify.success('ILUO Guadado Correctamente.');
            })
        }


        function refresh() {
            if ($scope.oEmployee && $scope.oEmployee.id > 0) {
                ctrl.load('EmployeeKey=' + $scope.oEmployee.id);
            } else {
                ctrl.load();
            }
        }

        $scope.openDC3Format = function(oEvent, oTrainingScore) {
            FormatDc3Service.getSingleWhere('TrainingScoreKey', oTrainingScore.id).then(function(data) {
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
                } else {
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

                        FormatDc3Service.createEntity({
                            EmployeeKey: $scope.oEmployee.id,
                            CertificationKey: oTrainingScore.Training.CatCertification.id,
                            TrainingScoreKey: oTrainingScore.id
                        }).then(function(oInstance) {
                            oInstance.Employee = $scope.oEmployee;
                            oInstance.Certification = oTrainingScore.Training.CatCertification;

                            $rootScope.$broadcast('load-modal-FormatDc3', oInstance, oTrainingScore);
                        });
                    });
                }
            });
        };

        $scope.toggleToPrint = function(trainingScore) {
            toPrint[trainingScore.id] = trainingScore.toPrint;
        };

        var toPrint = {};

        $scope.printSelected = function() {
            var selected = Object.getOwnPropertyNames(toPrint).filter(prop => toPrint[prop]);
            $location.path('/dc-3-report/' + selected.join());
        }

    });
