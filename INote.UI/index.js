var apiUrl = "http://localhost:59480/";

var app = angular.module("myApp", ["ngRoute"]);

app.config(function ($routeProvider) {
    $routeProvider.when("/", {
        templateUrl: "Pages/Main.html",
        controller: "mainCtrl"

    }).when("/register", {
        templateUrl: "Pages/Register.html",
        controller: "registerCtrl"
    }).when("/login", {
        templateUrl: "Pages/Login.html",
        controller: "loginCtrl"
    });

});
app.controller("mainCtrl", function ($scope) {
    $scope.message = "Anasayfadasınız.."

});

app.controller("registerCtrl", function ($scope, $http) {
    $scope.errors = [];
    $scope.successMessage = "";
    $scope.user = {
        Email: "erol.saroglu@hotmail.com",
        Password:"Erol1.",
        ConfirmPassword: "Erol1."
    };
    $scope.register = function (e) {
        $scope.errors = [];
        e.preventDefault();
        $http.post(apiUrl + "api/Account/Register", $scope.user).then(function (response) {
            $scope.user = { Email: "", Password: "", ConfirmPassword: "" };
            $scope.successMessage = "kayıt başarılı, şimdi giriş sayfasından giriş yapabilirsiniz.";
        }, function (response) {
            $scope.errors = getErrors(response.data.ModelState);
        });

    }
    $scope.hasErrors = function () {
        return $scope.errors.length > 0;
    }

});

app.controller("loginCtrl", function ($scope) {
    $scope.message = "Giriş Yap"

});

function getErrors(modelState) {
    var errors = [];
    for (var key in modelState) {
        for (var i = 0; i < modelState[key].length; i++) {
            errors.push(modelState[key][i]);
            if (modelState[key][i].includes("zaten alınmış.")) {
                break;
            }
        }
    }
    return errors;
}