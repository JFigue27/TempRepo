'use strict';

/**
 * @ngdoc function
 * @name Training.service:CertificationService
 * @description
 * # CertificationService
 * Service of the Training
 */
angular.module('Training').service('CertificationService', function(crudFactory) {

    var crudInstance = new crudFactory({

        entityName: 'Certification',

        catalogs: [],

        adapter: function(theEntity) {

            theEntity.SelectedProductionLines = [];
            if (theEntity.ProductionLines) {
                theEntity.SelectedProductionLines = theEntity.ProductionLines;
            }
            theEntity.sProductionLines = theEntity.SelectedProductionLines
                .map(function(current) {
                    return '' + current.Value;
                })
                .join('; ');
            if (theEntity.sProductionLines == '' || theEntity.sProductionLines == '[]') {
                theEntity.sProductionLines = '(Empty)';
            }

            return theEntity;
        },

        adapterIn: function(theEntity) {

        },

        adapterOut: function(theEntity, self) {
            theEntity.ProductionLines = theEntity.SelectedProductionLines;
        },

        dependencies: [

        ]
    });

    return crudInstance;
});
