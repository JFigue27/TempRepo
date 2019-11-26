'use strict';

/**
 * @ngdoc function
 * @name Training.controller:Dc3Controller
 * @description
 * # Dc3Controller
 * Controller of the Training
 */
angular.module('Training').controller('Dc3Controller', 
    function($scope, formController, mdDialog, Dc3Service) {
    
    var ctrl = this;

    formController.call(this, {
        scope: $scope,
        entityName: 'Dc3',
        baseService: Dc3Service,
        afterCreate: function(oEntity) {

        },
        afterLoad: function(oEntity) {

        }
    });

    $scope.$on('load-modal-Dc3', function(scope, oDc3) {
        refresh(oDc3);
    });

    $scope.$on('ok-modal-Dc3', function() {
        $scope.baseEntity.editMode = true;
        return ctrl.save().then(function() {
            $mdDialog.hide();
            alertify.success('Saved Successfully.');
        });
    });

    function refresh(oDc3) {
        ctrl.load(oDc3);
    }
    
});
