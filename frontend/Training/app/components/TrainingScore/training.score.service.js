'use strict';

/**
 * @ngdoc function
 * @name Training.service:TrainingScoreService
 * @description
 * # TrainingScoreService
 * Service of the Training
 */
angular.module('Training').service('TrainingScoreService', function(crudFactory, TrainingService) {

    var crudInstance = new crudFactory({
        
        entityName: 'TrainingScore',

        catalogs: [],

        adapter: function(theEntity) {
            if (theEntity.Training) {
                TrainingService.adapt(theEntity.Training);
            }
            theEntity.Employee.EmployeeNameAndNumber = theEntity.Employee.PersonalNumber + ' ' + theEntity.Employee.Name + theEntity.Employee.LastName + ' ' + theEntity.Employee.MotherLastName;
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