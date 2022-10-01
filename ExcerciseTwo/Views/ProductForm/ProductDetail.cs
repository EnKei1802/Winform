using ExcerciseTwo.Connection;
using ExcerciseTwo.Models;
using ExcerciseTwo.Repositorys;
using ExcerciseTwo.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace ExcerciseTwo.ProductForm
{
    public partial class ProductDetail : Form
    {
        private ICategoryRepository _categoryRepository;
        private IProductRepository _productRepository;
        private ListProduct _listProductForm;
        private List<Category> _categories;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="productRepository"></param>
        /// <param name="categoryRepository"></param>
        public ProductDetail(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _categories = _categoryRepository.GetAll();
            InitializeComponent();
            LoadCategory();

        }
        #region

        /// <summary>
        /// LoadCategory
        /// </summary>
        private void LoadCategory()
        {
            categoryLookup.Properties.DataSource = _categories;
        }

        /// <summary>
        /// photoButtonEdit_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void photoButtonEdit_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var image = new Bitmap(openFileDialog.FileName);
                photoPictureEdit.Image = image;
                photoButtonEdit.Text = openFileDialog.FileName;
            }

        }

        /// <summary>
        /// newButton_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(idText.Text))
                {
                    ClearData();
                    nameText.Focus();
                }
                else
                {
                    if (!ValidateForm()) return;
                    photoPictureEdit.EditValue = ImageUtility.UploadImage(photoButtonEdit.Text);
                    var product = CreateProduct(Convert.ToInt32(categoryLookup.EditValue));
                    bool isSuccess = false;
                    isSuccess = _productRepository.Post(product);
                    if (isSuccess)
                    {
                        MessageBox.Show("Create successfull !!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearData();
                        nameText.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Create failed !!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        /// <summary>
        /// Clear data
        /// </summary>
        private void ClearData()
        {
            idText.Text = string.Empty;
            nameText.Text = string.Empty;
            descriptionText.Text = string.Empty;
            quantitySpinEdit.Text = "0";
            createdDate.Text = string.Empty;
            activeCheck.Checked = false;
            typeRadioGroup.EditValue = null;
            photoButtonEdit.Text = string.Empty;
            priceText.Text = string.Empty;
            photoPictureEdit.Image = null;
            categoryLookup.EditValue = null;
        }

        /// <summary>
        ///  saveButton_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateForm()) return;
                photoPictureEdit.EditValue = ImageUtility.UploadImage(photoButtonEdit.Text);
                var product = CreateProduct(Convert.ToInt32(categoryLookup.EditValue));
                bool isSuccess = false;
                isSuccess = _productRepository.Put(product.productId, product);
                if (isSuccess)
                {
                    MessageBox.Show("Save successfull !!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearData();
                    saveButton.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Save failed !!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// deleteButton_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(idText.Text.Trim())) return;
                bool isSuccess = _productRepository.Delete(Convert.ToInt32(idText.Text));
                if (isSuccess)
                {
                    string pathImage = ImageUtility.UploadImage(photoButtonEdit.Text);
                    ImageUtility.RemoveImage(pathImage);
                    ClearData();
                    MessageBox.Show("Delete successfull !!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Delete failed !!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Cancel_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            _listProductForm = Program.serviceProvider.GetRequiredService<ListProduct>();
            _listProductForm.ShowDialog();
        }

        /// <summary>
        /// LoadDataFormListProduct
        /// </summary>
        public void LoadDataFormListProduct()
        {
            if (ListProduct.Product != null)
            {
                idText.EditValue = ListProduct.Product.productId;
                nameText.Text = ListProduct.Product.name;
                descriptionText.Text = ListProduct.Product.description;
                categoryLookup.EditValue = _categories.FirstOrDefault(x => x.categoryId == ListProduct.Product.categoryId)?.categoryId;
                quantitySpinEdit.EditValue = ListProduct.Product.quantity;
                createdDate.EditValue = ListProduct.Product.createdDate;
                activeCheck.Checked = ListProduct.Product.isActive;
                typeRadioGroup.EditValue = (int)ListProduct.Product.type;
                photoButtonEdit.Text = ImageUtility.GetFullImagePath(ListProduct.Product.photo);
                photoPictureEdit.Image = new Bitmap(photoButtonEdit.Text);
                priceText.EditValue = ListProduct.Product.price;
            }
        }

        /// <summary>
        /// CreateProduct
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private Product CreateProduct(int categoryId)
        {
            Product product = new Product();
            product.name = nameText.Text.Trim();
            product.description = descriptionText.Text.Trim();
            product.price = Convert.ToDecimal(priceText.Text);
            product.quantity = Convert.ToInt32(quantitySpinEdit.Text);
            product.createdDate = Extensitons.ConvertDateEditToDateTime(createdDate);
            product.isActive = activeCheck.Checked;
            product.type = (ProductType)typeRadioGroup.EditValue;
            product.photo = photoPictureEdit.Text.Trim();
            product.categoryId = categoryId;
            return product;
        }

        #endregion

        #region validate
        private bool ValidateForm()
        {
            return ValidateName()
                && ValidateDescription()
                && ValidateCategory()
                && ValidateQuantity()
                && ValidateCreatedDate()
                && ValidateType()
                && ValidatePhoto()
                && ValidatePrice();
        }
        private bool ValidateName()
        {
            if (string.IsNullOrEmpty(nameText.Text.Trim()))
            {
                nameText.Focus();
                errorProviderProductDetail.SetError(this.nameText, "Please fill the name");
                return false;
            }
            else
            {
                errorProviderProductDetail.Clear();
                return true;
            }
        }
        private bool ValidateDescription()
        {
            if (string.IsNullOrEmpty(descriptionText.Text.Trim()))
            {
                descriptionText.Focus();
                errorProviderProductDetail.SetError(this.descriptionText, "Please fill the description");
                return false;
            }
            else
            {
                errorProviderProductDetail.Clear();
                return true;
            }
        }
        private bool ValidateCategory()
        {
            if (categoryLookup.EditValue == null)
            {
                categoryLookup.Focus();
                errorProviderProductDetail.SetError(this.categoryLookup, "Please select the category");
                return false;
            }
            else
            {
                errorProviderProductDetail.Clear();
                return true;
            }
        }
        private bool ValidateQuantity()
        {
            if (Convert.ToInt32(quantitySpinEdit.Text) == 0)
            {
                quantitySpinEdit.Focus();
                errorProviderProductDetail.SetError(this.quantitySpinEdit, "Please fill the quantity");
                return false;
            }
            else
            {
                errorProviderProductDetail.Clear();
                return true;
            }
        }
        private bool ValidateCreatedDate()
        {
            if (string.IsNullOrEmpty(createdDate.Text))
            {
                createdDate.Focus();
                errorProviderProductDetail.SetError(this.createdDate, "Please fill the createdate");
                return false;
            }
            else
            {
                errorProviderProductDetail.Clear();
                return true;
            }
        }
        private bool ValidateType()
        {
            if (typeRadioGroup.EditValue == null)
            {
                errorProviderProductDetail.SetError(this.typeRadioGroup, "Please select the type");
                return false;
            }
            else
            {
                errorProviderProductDetail.Clear();
                return true;
            }
        }
        private bool ValidatePhoto()
        {
            if (string.IsNullOrEmpty(photoButtonEdit.Text))
            {
                errorProviderProductDetail.SetError(this.photoButtonEdit, "Please choose the photo");
                return false;
            }
            else
            {
                errorProviderProductDetail.Clear();
                return true;
            }
        }
        private bool ValidatePrice()
        {
            if (string.IsNullOrEmpty(priceText.Text.Trim()))
            {
                errorProviderProductDetail.SetError(this.priceText, "Please fill the price");
                return false;
            }
            else
            {
                errorProviderProductDetail.Clear();
                return true;
            }
        }

        private void priceText_Leave(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(priceText.Text))
            {
                int value;
                bool isConverted = Int32.TryParse(priceText.Text.Trim(), out value);
                if (!isConverted)
                {
                    MessageBox.Show("Only numbers allowed");
                    priceText.Focus();
                    return;
                }
            }    
        }

        #endregion
    }
}
