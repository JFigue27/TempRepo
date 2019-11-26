'use strict';

/**
 * @ngdoc function
 * @name Training.controller:SkillListController
 * @description
 * # SkillListController
 * Controller of the Training
 */
angular.module('Training').controller('SkillListController', function($scope, listController, SkillService) {

    var ctrl = new listController({
        scope: $scope,
        entityName: 'Skill',
        baseService: SkillService,
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
