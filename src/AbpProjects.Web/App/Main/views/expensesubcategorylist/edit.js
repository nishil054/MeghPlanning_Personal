(function () {
    angular.module('app').controller('app.views.expensesubcategorylist.edit', [
        '$scope', '$uibModalInstance', 'abp.services.app.expSubCategory', 'id',
        function ($scope, $uibModalInstance, expSubCategoryService, id) {
            var vm = this;
           
            vm.sname = [];
            $scope.btndisable = false;
            vm.saving = false;
           
            vm.cate = {};

            var init = function () {
                
               
                expSubCategoryService.getExpenseCategoryForDetail({ id: id })
                    .then(function (result) {
                        vm.cate = result.data;

                        vm.cate.categoryId = result.data.categoryId + "";
                        /*vm.cate.category = result.data.category + "";*/
                       
                        vm.cate.subCategory = result.data.subCategory + "";
                        console.log(vm.cate);
                        getCategoryName();
                       
                    });
            }

        



           
            vm.save = function () {
                $scope.btndisable = true;
                vm.saving = true;
                expSubCategoryService.expenseSubCategoryExsistenceById(vm.cate).then(function (result) {
                    if (!result.data) {
                        expSubCategoryService.updateExpenseSubCategory(vm.cate).then(function () {
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
          
           
            init();
            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

        }
    ]);
})();