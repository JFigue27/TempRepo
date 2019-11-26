'use strict';

/**
 * @ngdoc function
 * @name Training.service:MatrixService
 * @description
 * # MatrixService
 * Service of the Training
 */
angular.module('Training').service('MatrixService', function(crudFactory) {

    var crudInstance = new crudFactory({
        
        entityName: 'Matrix',

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