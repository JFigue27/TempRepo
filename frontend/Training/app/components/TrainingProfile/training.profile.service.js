'use strict';

/**
 * @ngdoc function
 * @name Training.service:TrainingProfileService
 * @description
 * # TrainingProfileService
 * Service of the Training
 */
angular.module('Training').service('TrainingProfileService', function(crudFactory) {

    var crudInstance = new crudFactory({
        
        entityName: 'TrainingProfile',

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