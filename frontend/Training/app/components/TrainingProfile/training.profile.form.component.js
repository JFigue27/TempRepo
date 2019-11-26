'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:TrainingProfile
 * @description
 * # TrainingProfile
 */
angular.module('Training').directive('trainingProfileForm', function() {
    return {
        templateUrl: 'components/TrainingProfile/training.profile.form.html',
        restrict: 'E',
        controller: function($scope, formController, $mdDialog, TrainingProfileService) {

            var ctrl = this;

            formController.call(this, {
                scope: $scope,
                entityName: 'TrainingProfile',
                baseService: TrainingProfileService,
                afterCreate: function(oEntity) {

                },
                afterLoad: function(oEntity) {

                }
            });

            $scope.$on('load-modal-TrainingProfile', function(scope, oTrainingProfile) {
                refresh(oTrainingProfile);
            });

            $scope.$on('ok-modal-TrainingProfile', function() {
                $scope.baseEntity.editMode = true;
                return ctrl.save().then(function() {
                    $mdDialog.hide();
                    alertify.success('Saved Successfully.');
                });
            });

            function refresh(oTrainingProfile) {
                ctrl.load(oTrainingProfile);
            }

        }
    };
});
