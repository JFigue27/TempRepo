'use strict';

/**
 * @ngdoc function
 * @name Training.controller:CertificationListController
 * @description
 * # CertificationListController
 * Controller of the Training
 */
angular.module('Training').controller('CertificationListController', function($scope, listController, CertificationService) {

    var ctrl = new listController({
        scope: $scope,
        entityName: 'Certification',
        baseService: CertificationService,
        afterCreate: function(oInstance) {

        },
        afterLoad: function() {

        },
        onOpenItem: function(oItem) {

        },
        filters: {
            
        }
    });

    ctrl.load();

});
