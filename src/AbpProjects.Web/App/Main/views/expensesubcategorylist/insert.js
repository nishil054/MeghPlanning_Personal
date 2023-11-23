(function () {
    angular.module('app').controller('app.views.expensesubcategorylist.insert', [
        '$scope', '$uibModalInstance', 'abp.services.app.expSubCategory',
        function ($scope, $uibModalInstance,  expSubCategoryService) {
            var vm = this;
           
            vm.catego = [];
           
            vm.saving = false;
            $scope.btndisable = false;
            vm.cate = {};
          
            vm.save = function () {
                $scope.btndisable = true;
                vm.saving = true;
               
                expSubCategoryService.expenseSubCategoryExsistence(vm.cate).then(function (result) {
                    if (!result.data) {
                        expSubCategoryService.expenseCreateSubCategory(vm.cate).then(function () {
                            abp.notify.info(App.localize('SavedSuccessfully'));
                            $uibModalInstance.close();
                        });
                    }
                    else {
                        abp.notify.error(App.localize('SubCategory Already Exist'));
                        $scope.btndisable = false;
                    }
                });

            };
            
            
            function getCategoryName() {
                expSubCategoryService.getCategory({}).then(function (result) {
                    vm.catego = result.data.items;
                    console.log(vm.catego);
                });
            }
            
           
            
            var init = function () {
                getCategoryName();
               
               
            }
            init();
            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

        }
    ]);
})();