'use strict';

/**
 * @ngdoc function
 * @name Training.service:EmployeeService
 * @description
 * # EmployeeService
 * Service of the Training
 */
angular.module('Training').service('EmployeeService', function(crudFactory) {

    var crudInstance = new crudFactory({

        entityName: 'Employee',

        catalogs: [],

        adapter: function(theEntity) {
            theEntity.EmployeeNameAndNumber = theEntity.PersonalNumber + ' ' + theEntity.Name + ' ' + theEntity.LastName + ' ' + theEntity.MotherLastName;
            
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
