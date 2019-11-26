'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Training
 * @description
 * # Training
 */
angular.module('Training').directive('trainingForm', function() {
    return {
        templateUrl: 'components/Training/training.form.html',
        restrict: 'E',
        scope: {},
        controller: function($scope, formController, $mdDialog, TrainingService, CertificationService, EmployeeService, LevelService) {

            var ctrl = this;

            formController.call(this, {
                scope: $scope,
                entityName: 'Training',
                baseService: TrainingService,
                afterCreate: function(oEntity) {

                },
                afterLoad: function(oEntity) {
                    $scope.$broadcast('load_TrainingScore');
                }
            });

            $scope.$on('load-modal-Training', function(scope, oTraining) {
                refresh(oTraining);
            });

            $scope.$on('ok-modal-Training', function() {
                $scope.baseEntity.editMode = true;
                $scope.baseEntity.TrainingScores = $scope.baseList;
                return ctrl.save().then(function() {
                    $mdDialog.hide();
                    alertify.success('Saved Successfully.');
                });
            });

            function refresh(oTraining) {
                ctrl.load(oTraining);
            }

            CertificationService.loadEntities(true).then(function(oResponse) {
                $scope.Certifications = oResponse.Result;
            });

            LevelService.loadEntities(true).then(function(oResponse) {
                $scope.theAreas = oResponse.Result;
            });

            EmployeeService.getFilteredPage(0, 1, "?JobPositionKey=1").then(function(oResponse) {
                $scope.theSupervisors = oResponse.Result;
            });

        }
    };
});
