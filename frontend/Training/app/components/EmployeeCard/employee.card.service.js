'use strict';

/**
 * @ngdoc function
 * @name Training.service:EmployeeCardService
 * @description
 * # EmployeeCardService
 * Service of the Training
 */
angular.module('Training').service('EmployeeCardService', function(crudFactory) {

    var crudInstance = new crudFactory({

        entityName: 'Employee',

        catalogs: [],

        adapter: function(theEntity) {

            theEntity.certifications = [];
            if (theEntity.TrainingScores) {
                theEntity.certifications = theEntity.TrainingScores.filter(function(score){
                    return score.Training.sys_active == true;
                }).map(function(score) {
                    return {    
                        Value: score.Training.CatCertification.Value,
                        DateExpiresAt: score.Training.DateExpiresAt,
                        DateCertified: score.Training.DateCertification,
                        AppliesToDC3: score.Training.CatCertification.AppliesToDC3,
                        VisibleToCard: score.Training.CatCertification.VisibleToCard
                    }
                })
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
