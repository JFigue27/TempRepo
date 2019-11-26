'use strict';

/**
 * @ngdoc function
 * @name Training.service:UserService
 * @description
 * # UserService
 * Service of the Training
 */
angular.module('Training').service('UserService', function(crudFactory) {

    var crudInstance = new crudFactory({
        
        entityName: 'User',

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