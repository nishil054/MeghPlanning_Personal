/* Used by user and role permission settings. */
(function () {
    var app = angular.module("app");
    app.directive('permissionTree', [
        function () {
            return {
                restrict: 'E',
                template: '<div class="permission-tree" id="plugins4"></div>',
                scope: {
                    editData: '='
                },
                link: function ($scope, element, attr) {
                    var $tree = $(element).find('.permission-tree');
                    var treeInitializedBefore = false;
                    var inTreeChangeEvent = false;
                    var to = false;

                    $scope.$watch('editData', function () {
                        if (!$scope.editData) {
                            return;
                        }

                        initializeTree();
                    });

                    function initializeTree() {
                        if (treeInitializedBefore) {
                            $tree.jstree('destroy');
                        }
                        var treeData = _.map($scope.editData.permissions, function (item) {
                            return {
                                id: item.name,
                                parent: item.parentName ? item.parentName : '#',
                                text: item.displayName,
                                state: {
                                    opened: true,
                                    selected: _.contains($scope.editData.grantedPermissionNames, item.name)
                                }
                            };
                        });

                        $tree.jstree({
                            'core': {
                                data: treeData
                            },
                            "types": {
                                "default": {
                                    "icon": "fa fa-folder tree-item-icon-color icon-lg"
                                },
                                "file": {
                                    "icon": "fa fa-file tree-item-icon-color icon-lg"
                                }
                            },
                            'checkbox': {
                                keep_selected_style: false,
                                three_state: false,
                                cascade: ''
                            },
                            "search": {
                                "show_only_matches": true
                                //"show_only_matches_children": true
                            },
                            plugins: ['checkbox', 'types', 'search']
                        });

                        treeInitializedBefore = true;

                        $tree.on("changed.jstree", function (e, data) {
                            if (!data.node) {
                                return;
                            }

                            var wasInTreeChangeEvent = inTreeChangeEvent;
                            if (!wasInTreeChangeEvent) {
                                inTreeChangeEvent = true;
                            }

                            var childrenNodes;

                            if (data.node.state.selected) {
                                selectNodeAndAllParents($tree.jstree('get_parent', data.node));

                                childrenNodes = $.makeArray($tree.jstree('get_children_dom', data.node));
                                $tree.jstree('select_node', childrenNodes);

                            } else {
                                childrenNodes = $.makeArray($tree.jstree('get_children_dom', data.node));
                                $tree.jstree('deselect_node', childrenNodes);
                            }

                            if (!wasInTreeChangeEvent) {
                                $scope.$apply(function () {
                                    $scope.editData.grantedPermissionNames = getSelectedPermissionNames();
                                });
                                inTreeChangeEvent = false;
                            }
                        });

                        $(function () {
                            $("#plugins4").jstree({
                                "plugins": ["search"]
                            });
                            var to = false;
                            $('#plugins4_q').keyup(function () {
                                if (to) { clearTimeout(to); }
                                to = setTimeout(function () {
                                    var v = $('#plugins4_q').val();
                                    $('#plugins4').jstree(false).search(v);
                                }, 150);
                            });
                        });
                        
                    };

                    function selectNodeAndAllParents(node) {
                        //debugger;
                        $tree.jstree('select_node', node, true);
                        var parent = $tree.jstree('get_parent', node);
                        if (parent) {
                            selectNodeAndAllParents(parent);
                        }
                    };

                    function getSelectedPermissionNames() {
                        //debugger;
                        var permissionNames = [];

                        var selectedPermissions = $tree.jstree('get_selected', true);
                        for (var i = 0; i < selectedPermissions.length; i++) {
                            permissionNames.push(selectedPermissions[i].original.id);
                        }

                        return permissionNames;
                    };
                }
            };
        }]);
})();