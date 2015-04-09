
// create our angular app and inject ngAnimate and ui-router 
// =============================================================================
angular.module('preRegApp', ['ngAnimate', 'ui.router', 'ngMessages'])

// configuring our routes 
// =============================================================================
.config(function ($stateProvider, $urlRouterProvider) {

    $stateProvider

        // route to show our basic form (/twoWheeler)
        .state('twoWheeler', {
            url: '/twoWheeler',
            abstract: true,
            templateUrl: 'app-view/pre-reg/vehicles/twoWheeler.html',
            controller: 'formController'
           
        })

        // nested states 
        // each of these sections will have their own view
        // url will be nested (/twoWheeler/vehicle)
        .state('twoWheeler.vehicle', {
            url: '',
            templateUrl: 'app-view/pre-reg/vehicles/twoWheeler-vehicle.html'
        })

        // url will be /twoWheeler/owner
        .state('twoWheeler.owner', {
            url: '/owner',
            templateUrl: 'app-view/pre-reg/vehicles/twoWheeler-owner.html'
        })

        // url will be /twoWheeler/purchase
        .state('twoWheeler.purchase', {
            url: '/purchase',
            templateUrl: 'app-view/pre-reg/vehicles/twoWheeler-purchase.html'
        })

    // url will be /twoWheeler/purchase
      .state('twoWheeler.insurance', {
          url: '/insurance',
          templateUrl: 'app-view/pre-reg/vehicles/twoWheeler-insurance.html'
    });

   // $urlRouterProvider.when('app-view/pre-reg/vehicles/twoWheeler.html', 'app-view/pre-reg/vehicles/twoWheeler-vehicle.html');

    // catch all route
    // send users to the form page 
    $urlRouterProvider.otherwise('/twoWheeler/vehicle');
})

// our controller for the form
// =============================================================================
.controller('formController', function ($scope, $http, $modal, $log) {

    $scope.formData = {};
    //$scope.cnVehicleState = 'Vehicle_State';
    //$scope.cnOwnerState = 'Owner_State';

    $http.get("/api/common/getTwoWheelerOption").success(function (data, status, headers, config) {
        
        $scope.vehicleMakeOptions = data.vehicleMakeOptions;
        $scope.countryOptions = data.countryOptions;
        $scope.manufactureYearOptions = data.manufactureYearOptions;
        $scope.ownerTypesOptions = data.ownerTypesOptions;
        $scope.ownerCountryOptions = data.ownerCountryOptions;
        


        $scope.title = data.title;
        $scope.answered = false;
        $scope.working = false;
    }).error(function (data, status, headers, config) {
        $scope.title = "Oops... something went wrong";
        $scope.working = false;
    });

    $scope.getStateByCountry = function (countryCode,targetDropdown) {
      

        $http.get("/api/common/getStateByCountry", { params: { countryCode: countryCode } }).success(function (data, status, headers, config) {

           
            switch (targetDropdown) {

                case "Vehicle_State":
                    $scope.stateOptions = data;
                    break;
                case "Owner_State":
                    $scope.ownerStateOptions = data;
                    break;
                case "PreviousOwner_State":
                    $scope.previousOwnerStateOptions = data;
                    break;
                case "Purchase_State":
                    $scope.purchaseStateOptions = data;
                    break;
                default:

            }
    
        }).error(function (data, status, headers, config) {
            $scope.title = "Oops... something went wrong";
            $scope.working = false;
        });

    };


    $scope.getDistricts = function (countryCode,stateCode ,targetDropdown) {


        $http.get("/api/common/getDistricts", { params: { countryCode: countryCode, stateCode: stateCode } }).success(function (data, status, headers, config) {


            switch (targetDropdown) {

                case "Owner_District":
                    $scope.ownerDistrictOptions = data;
                    $scope.ownerCityOptions = [];
                    break;
                case "PreviousOwner_District":
                    $scope.previousOwnerDistrictOptions = data;
                    $scope.previousOwnerCityOptions = [];
                    break;
                case "Purchase_District":
                    $scope.purchaseDistrictOptions = data;
                    $scope.purchaseCityOptions = [];
                    break;
                default:

            }

        }).error(function (data, status, headers, config) {
            $scope.title = "Oops... something went wrong";
            $scope.working = false;
        });

    };

    $scope.getCities = function (countryCode, stateCode,districtCode, targetDropdown) {


        $http.get("/api/common/getCities", { params: { countryCode: countryCode, stateCode: stateCode, districtCode: districtCode } }).success(function (data, status, headers, config) {


            switch (targetDropdown) {

                
                case "Owner_City":
                    $scope.ownerCityOptions = data;
                    break;
                case "PreviousOwner_City":
                    $scope.previousOwnerCityOptions = data;
                    break;
                case "Purchase_City":
                    $scope.purchaseCityOptions = data;
                    break;
                default:

            }

        }).error(function (data, status, headers, config) {
            $scope.title = "Oops... something went wrong";
            $scope.working = false;
        });

    };


    

    $scope.getVehicleNoByState = function (stateCode) {

        $http.get("/api/common/getVehicleNoByState", { params: { stateCode: stateCode } }).success(function (data, status, headers, config) {

            $scope.vehicleNoOptions = data;

        }).error(function (data, status, headers, config) {
            $scope.title = "Oops... something went wrong";
            $scope.working = false;
        });

    };



    // function to process the form
    $scope.processForm = function (valid, formData) {
       
        if (valid) {
            isloading = true;
            $http.post('/api/twowheeler', formData).success(function (data, status, headers, config) {

                if (data.isSuccess) {
                    $scope.addAlert("success", "Your Vehicle registeration is completed successfully.Please check email for more details");
                }
                else {
                    $scope.addAlert("danger", data.message);
                }

            }).error(function (data, status, headers, config) {

                $scope.addAlert("danger", "Oops... something went wrong");

            });
            isloading = false;
        }
        else {
            $scope.addAlert("danger", "You must fill in all of the required fields.");
        }

    };

   
    $scope.dpOpened = {
        opened1: false,
        opened2: false,
        opened2:false
    };

    $scope.openDatePicker = function ($event, opened) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.dpOpened[opened] = true;
    };

    $scope.open = function () {

        var modalInstance = $modal.open({
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            size: 'lg'
            
        });
        
    };

    //$scope.alerts = [
    //{ type: 'danger', msg: 'Oh snap! Change a few things up and try submitting again.' },
    //{ type: 'success', msg: 'Well done! You successfully read this important alert message.' }
    //];

    $scope.alerts = [];

    $scope.addAlert = function (type,msg) {
        $scope.alerts.push({type:type, msg: msg });
    };

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };
   

});

angular.module('preRegApp').controller('ModalInstanceCtrl', function ($scope, $modalInstance) {


    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
});





