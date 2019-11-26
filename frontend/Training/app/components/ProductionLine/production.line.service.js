'use strict';

/**
 * @ngdoc function
 * @name Training.service:ProductionLineService
 * @description
 * # ProductionLineService
 * Service of the Training
 */
angular.module('Training').service('ProductionLineService', function(crudFactory) {

    var crudInstance = new crudFactory({

        entityName: 'ProductionLine',

        catalogs: [],

        adapter: function(theEntity) {

            theEntity.SelectedCertifications = [];
            if (theEntity.Certifications) {
                theEntity.SelectedCertifications = theEntity.Certifications;
            }
            theEntity.sCertifications = theEntity.SelectedCertifications
                .map(function(current) {
                    return '' + current.Value;
                })
                .join('; ');
            if (theEntity.sCertifications == '' || theEntity.sCertifications == '[]') {
                theEntity.sCertifications = '(Empty)';
            }

            return theEntity;
        },

        adapterIn: function(theEntity) {

        },

        adapterOut: function(theEntity, self) {
            theEntity.Certifications = theEntity.SelectedCertifications;
        },

        dependencies: [

        ]
    });

    return crudInstance;
});
