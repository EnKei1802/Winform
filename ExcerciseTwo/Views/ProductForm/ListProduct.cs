using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using ExcerciseTwo.Models;
using ExcerciseTwo.Repositorys;
using ExcerciseTwo.Utilities;

namespace ExcerciseTwo.ProductForm
{
    public partial class ListProduct : Form
    {
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;
        private List<Product> _products;
        private List<Category> _categorys;
        private ProductDetail _productDetailForm;

        private List<Product> ListProductUpdate = new List<Product>();
        private int rowIndex = -1;

        public static Product? Product;
        public Product ProductFocused;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="productRepository"></param>
        /// <param name="categoryRepository"></param>
        /// <param name="productDetailForm"></param>
        public ListProduct(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            ProductDetail productDetailForm)
        {
            InitializeComponent();
            _productDetailForm = productDetailForm;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _products = _productRepository.GetAll();
            _categorys = _categoryRepository.GetAll();
            LoadCategoryData();
            LoadProductData();
            DisableAllButtonBeforePickRow();

        }

        #region
        /// <summary>
        /// Hide all button when pick on the row
        /// </summary>
        private void DisableAllButtonBeforePickRow()
        {
            newProductButton.Enabled = false;
            saveButton.Enabled = false;
            editButton.Enabled = false;
            deleteButton.Enabled = false;
        }

        /// <summary>
        /// Unhide all button when pick on the row
        /// </summary>
        private void EnableAllButtonBeforePickRow()
        {
            newProductButton.Enabled = true;
            editButton.Enabled = true;
            deleteButton.Enabled = true;
        }

        /// <summary>
        /// Load category data
        /// </summary>
        private void LoadCategoryData()
        {
            var categories = _categorys;
            categoryItems.DataSource = categories;
            categoryItems.DisplayMember = Constants.CATEGORY_NAME;
            categoryItems.ValueMember = Constants.CATEGORY_ID;
        }

        /// <summary>
        /// Load product data
        /// </summary>
        private void LoadProductData()
        {
            gridControlProducts.DataSource = _products;
        }

        /// <summary>
        /// New product click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newProductButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            _productDetailForm.ShowDialog();
        }


        /// <summary>
        /// gridViewProduct_DoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewProduct_DoubleClick(object sender, EventArgs e)
        {
            var product = GetProductRow(sender, e);
            if (product != null)
            {
                Product = _products.FirstOrDefault(p => p.productId == product.productId);
                this.Hide();
                _productDetailForm.LoadDataFormListProduct();
                _productDetailForm.ShowDialog();
            }
        }


        /// <summary>
        /// gridViewProduct_RowClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewProduct_RowClick(object sender, RowClickEventArgs e)
        {
            if (e.RowHandle == rowIndex) return;
            rowIndex = e.RowHandle;
            var product = GetProductRow(sender, e);
            ProductFocused = product;
            if (product != null)
            {
                typeText.Text = product.type.ToString();
                quantityText.Text = product.quantity.ToString();
                photoPictureBox.Image = new Bitmap(ImageUtility.GetFullImagePath(product.photo));
                EnableAllButtonBeforePickRow();
            }
        }


        /// <summary>
        /// Get product row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private Product GetProductRow(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            if (view is null) return null;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            return view.GetRow(info.RowHandle) as Product;
        }

        /// <summary>
        /// editButton_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editButton_Click(object sender, EventArgs e)
        {
            var product = ProductFocused;
            if (product != null)
            {
                Product = _products.FirstOrDefault(p => p.productId == product.productId);
                this.Hide();
                _productDetailForm.LoadDataFormListProduct();
                _productDetailForm.ShowDialog();
            }
        }

        /// <summary>
        ///  gridViewProduct_CellValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewProduct_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            var listProductView = sender as GridView;
            var columnChanged = e.Column.FieldName;

            var dataUpdated = listProductView.GetRow(e.RowHandle) as Product;
            if (dataUpdated != null)
            {
                if (dataUpdated.Equals(ProductFocused)) return;

                var index = ListProductUpdate.FindIndex(x => x.productId == dataUpdated.productId);
                if (index != -1)
                    ListProductUpdate[index] = dataUpdated;
                ListProductUpdate.Add(dataUpdated);
            }
        }

        /// <summary>
        /// saveButton_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                ListProductUpdate.ForEach(x => _productRepository.Put(x.productId, x));
                ListProductUpdate.Clear();
                saveButton.Enabled = false;
                MessageBox.Show("Save successfull !!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Refresh();
            }catch(Exception ex)
            {
                throw;
            }
           
        }

        /// <summary>
        /// gridViewProduct_CellValueChanging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewProduct_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            saveButton.Enabled = true;
        }
    }
    #endregion
}
