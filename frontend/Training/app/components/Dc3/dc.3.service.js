'use strict';

/**
 * @ngdoc function
 * @name Training.service:Dc3Service
 * @description
 * # Dc3Service
 * Service of the Training
 */
angular.module('Training').service('Dc3Service', function(crudFactory) {

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