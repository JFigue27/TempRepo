'use strict';

/**
 * @ngdoc function
 * @name Training.service:ShiftService
 * @description
 * # ShiftService
 * Service of the Training
 */
angular.module('Training').service('ShiftService', function(crudFactory) {

    var crudInstance = new crudFactory({
        
        entityName: 'Shift',

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