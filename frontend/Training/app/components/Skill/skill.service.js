'use strict';

/**
 * @ngdoc function
 * @name Training.service:SkillService
 * @description
 * # SkillService
 * Service of the Training
 */
angular.module('Training').service('SkillService', function(crudFactory) {

    var crudInstance = new crudFactory({
        
        entityName: 'Skill',

        catalogs: [],

        adapter: function(theEntity) {
            return theEntity;
        },

        adapterIn: function(theEntity) {

        },

        adapterOut: function(theEntity, self) {

        },

        dependencies: [

        ]
    });

    return crudInstance;
});