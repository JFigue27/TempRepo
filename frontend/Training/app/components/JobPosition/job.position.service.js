'use strict';

/**
 * @ngdoc function
 * @name Training.service:JobPositionService
 * @description
 * # JobPositionService
 * Service of the Training
 */
angular.module('Training').service('JobPositionService', function(crudFactory) {

    var crudInstance = new crudFactory({
        
        entityName: 'JobPosition',

        catalogs: [],

        adapter: function(theEntity) {

            theEntity.SelectedSkills = [];
            if (theEntity.Skills) {
                theEntity.SelectedSkills = theEntity.Skills;
            }
            theEntity.sSkills = theEntity.SelectedSkills
                .map(function(current) {
                    return '' + current.Value;
                })
                .join('; ');
            if (theEntity.sSkills == '' || theEntity.sSkills == '[]') {
                theEntity.sSkills = '(Empty)';
            }

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