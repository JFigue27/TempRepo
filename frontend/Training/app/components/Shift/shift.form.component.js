'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Shift
 * @description
 * # Shift
 */
angular.module('Training').directive('shiftForm', function() {
    return {
        templateUrl: 'components/Shift/shift.form.html',
        restrict: 'E',
        controller: function($scope, formController, $mdDialog, ShiftService) {

            var ctrl = this;

            formController.call(this, {
                scope: $scope,
                entityName: 'Shift',
                baseService: ShiftService,
                afterCreate: function(oEntity) {

                },
                afterLoad: function(oEntity) {

                }
            });

            $scope.$on('load-modal-Shift', function(scope, oShift) {
                refresh(oShift);
            });

            $scope.$on('ok-modal-Shift', function() {
                $scope.baseEntity.editMode = true;
                return ctrl.save().then(function() {
                    $mdDialog.hide();
                    alertify.success('Saved Successfully.');
                });
            });

            function refresh(oShift) {
                ctrl.load(oShift);
            }

        }
    };
});
