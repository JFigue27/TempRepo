'use strict';

/**
 * @ngdoc function
 * @name Training.service:FormatDc3Service
 * @description
 * # FormatDc3Service
 * Service of the Training
 */
angular.module('Training').service('FormatDc3Service', function(crudFactory) {

    var crudInstance = new crudFactory({
        
        entityName: 'FormatoDC3',

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