'use strict';

/**
 * @ngdoc function
 * @name Training.service:WorkstationService
 * @description
 * # WorkstationService
 * Service of the Training
 */
angular.module('Training').service('WorkstationService', function(crudFactory) {

    var crudInstance = new crudFactory({
        
        entityName: 'Workstation',

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