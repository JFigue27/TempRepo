/**
 * @ngdoc directive
 * @name Training.directive:FormatDc3
 * @description
 * # FormatDc3
 */
angular.module('Training').directive('formatDc3Form', function() {
    return {
        templateUrl: 'components/FormatDc3/format.dc.3.form.html',
        restrict: 'E',
        scope: {
            identifier:'='
        },
        controller: function($scope, formController, $mdDialog, FormatDc3Service, TrainingService) {

            var ctrl = this;

            formController.call(this, {
                scope: $scope,
                entityName: 'FormatDc3',
                baseService: FormatDc3Service,
                afterCreate: function(oEntity) {

                },
                afterLoad: function(oEntity) {

                    if (!$scope.TrainingScore) {
                        $scope.TrainingScore = oEntity.TrainingScore;
                    }

                    if ($scope.TrainingScore && $scope.TrainingScore.Training) {
                        TrainingService.adapt($scope.TrainingScore.Training);
                        $scope.FromYear = $scope.TrainingScore.Training.ConvertedDateStart && moment($scope.TrainingScore.Training.ConvertedDateStart).format('Y');
                        $scope.FromMonth = $scope.TrainingScore.Training.ConvertedDateStart && moment($scope.TrainingScore.Training.ConvertedDateStart).format('MM');
                        $scope.FromDay = $scope.TrainingScore.Training.ConvertedDateStart && moment($scope.TrainingScore.Training.ConvertedDateStart).format('DD');
                        $scope.ToYear = $scope.TrainingScore.Training.ConvertedDateEnd && moment($scope.TrainingScore.Training.ConvertedDateEnd).format('Y');
                        $scope.ToMonth = $scope.TrainingScore.Training.ConvertedDateEnd && moment($scope.TrainingScore.Training.ConvertedDateEnd).format('MM');
                        $scope.ToDay = $scope.TrainingScore.Training.ConvertedDateEnd && moment($scope.TrainingScore.Training.ConvertedDateEnd).format('DD');
                    }
                }
            });

            $scope.$on('load-modal-FormatDc3', function(scope, oFormatDc3, oTrainingScore) {
                $scope.TrainingScore = oTrainingScore;
                refresh(oFormatDc3);
            });

            $scope.$on('ok-modal-FormatDc3', function() {
                $scope.baseEntity.editMode = true;
                return ctrl.save().then(function() {
                    $mdDialog.hide();
                    alertify.success('Saved Successfully.');
                });
            });

            function refresh(oFormatDc3) {
                if ($scope.oTrainingScore && $scope.TrainingScore > 0) {
                    FormatDc3Service.getSingleWhere('TrainingScoreKey', $scope.TrainingScore.id).then(function(data) {
                        if (data && data.id > 0) {
                            ctrl.load(data);
                        }
                    });
                } else {
                    ctrl.load(oFormatDc3);
                }
            }

            $scope.$on('print-modal-FormatDc3', function() {
                window.open('reports/DC-3-training-profile.html');
            });

            $scope.$watch('identifier', function() {
                if($scope.identifier) {
                    FormatDc3Service.getSingleWhere('TrainingScoreKey', $scope.identifier).then(function(response){
                        refresh(response);
                    });
                }
            })

        }
    };
});
