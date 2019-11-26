'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:TrainingScore
 * @description
 * # TrainingScore
 */
angular.module('Training').directive('trainingScoreForm', function() {
    return {
        templateUrl: 'components/TrainingScore/training.score.form.html',
        restrict: 'E',
        controller: function($scope, formController, $mdDialog, TrainingScoreService) {

            var ctrl = this;

            formController.call(this, {
                scope: $scope,
                entityName: 'TrainingScore',
                baseService: TrainingScoreService,
                afterCreate: function(oEntity) {

                },
                afterLoad: function(oEntity) {

                }
            });

            $scope.$on('load-modal-TrainingScore', function(scope, oTrainingScore) {
                refresh(oTrainingScore);
            });

            $scope.$on('ok-modal-TrainingScore', function() {
                $scope.baseEntity.editMode = true;
                return ctrl.save().then(function() {
                    $mdDialog.hide();
                    alertify.success('Saved Successfully.');
                });
            });

            function refresh(oTrainingScore) {
                ctrl.load(oTrainingScore);
            }

        }
    };
});
