'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Matrix
 * @description
 * # Matrix
 */
angular.module('Training').directive('matrixForm', function() {
    return {
        templateUrl: 'components/Matrix/matrix.form.html',
        restrict: 'E',
        controller: function($scope, formController, $mdDialog, MatrixService) {

            var ctrl = this;

            formController.call(this, {
                scope: $scope,
                entityName: 'Matrix',
                baseService: MatrixService,
                afterCreate: function(oEntity) {

                },
                afterLoad: function(oEntity) {

                }
            });

            $scope.$on('load-modal-Matrix', function(scope, oMatrix) {
                refresh(oMatrix);
            });

            $scope.$on('ok-modal-Matrix', function() {
                $scope.baseEntity.editMode = true;
                return ctrl.save().then(function() {
                    $mdDialog.hide();
                    alertify.success('Saved Successfully.');
                });
            });

            function refresh(oMatrix) {
                ctrl.load(oMatrix);
            }

        }
    };
});
