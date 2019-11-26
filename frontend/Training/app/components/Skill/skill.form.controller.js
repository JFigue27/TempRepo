'use strict';

/**
 * @ngdoc function
 * @name Training.controller:SkillController
 * @description
 * # SkillController
 * Controller of the Training
 */
angular.module('Training').controller('SkillController', function($scope, formController, SkillService) {
    
    var ctrl = this;

    formController.call(this, {
        scope: $scope,
        entityName: 'Skill',
        baseService: SkillService,
        afterCreate: function(oEntity) {

        },
        afterLoad: function(oEntity) {

        }
    });
	
	ctrl.load();
});
