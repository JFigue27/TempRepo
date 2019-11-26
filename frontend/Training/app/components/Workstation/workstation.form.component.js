'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Workstation
 * @description
 * # Workstation
 */
angular.module('Training').directive('workstationForm', function() {
    return {
        templateUrl: 'components/Workstation/workstation.form.html',
        restrict: 'E',
        controller: function($scope, formController, $mdDialog, WorkstationService) {

            var ctrl = this;

            formController.call(this, {
                scope: $scope,
                entityName: 'Workstation',
                baseService: WorkstationService,
                afterCreate: function(oEntity) {

                },
                afterLoad: function(oEntity) {

                }
            });

            $scope.$on('load-modal-Workstation', function(scope, oWorkstation) {
                refresh(oWorkstation);
            });

            $scope.$on('ok-modal-Workstation', function() {
                $scope.baseEntity.editMode = true;
                return ctrl.save().then(function() {
                    $mdDialog.hide();
                    alertify.success('Saved Successfully.');
                });
            });

            function refresh(oWorkstation) {
                ctrl.load(oWorkstation);
            }

        }
    };
});
