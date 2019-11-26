'use strict';

/**
 * @ngdoc function
 * @name Training.controller:CertificationController
 * @description
 * # CertificationController
 * Controller of the Training
 */
angular.module('Training').controller('CertificationController', function($scope, formController, CertificationService) {
    
    var ctrl = this;

    formController.call(this, {
        scope: $scope,
        entityName: 'Certification',
        baseService: CertificationService,
        afterCreate: function(oEntity) {

        },
        afterLoad: function(oEntity) {

        }
    });
	
	ctrl.load();
});
