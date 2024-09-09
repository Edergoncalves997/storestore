<template>
  <div>
    <!-- Cabeçalho da página -->
    <div class="jumbotron p-3 p-md-5 text-white rounded bg-dark">
      <div class="col-md-6 px-0">
        <h1 class="display-4 font-italic">{{ msg }}</h1>
      </div>
    </div>

    <!-- Formulário para adicionar ou editar um produto -->
    <form @submit.prevent="isEditing ? updateProduct() : addProduct()">
      <div class="form-group">
        <label for="name">Produto</label>
        <input type="text" id="name" v-model="currentProduct.title" class="form-control" required />
      </div>

      <div class="form-group">
        <label for="price">Preço</label>
        <input type="number" id="price" v-model="currentProduct.price" class="form-control" required />
      </div>

      <div class="form-group">
        <label for="barcode">Código de Barras</label>
        <input type="text" id="barcode" v-model="currentProduct.barcode" class="form-control" required />
      </div>
    
      <div class="form-group">
        <label for="image">URL da Imagem</label>
        <input type="text" id="image" v-model="currentProduct.image" class="form-control" required />
      </div>

      <button type="submit" class="btn btn-primary">{{ isEditing ? 'Atualizar Produto' : 'Adicionar Produto' }}</button>
    </form>

    <!-- Formulário de pesquisa -->
    <div class="form-group mt-4">
      <label for="search">Pesquisar</label>
      <input type="text" id="search" v-model="searchQuery" class="form-control" @input="filterProducts" placeholder="Digite Nome ou Código de Barras" />
    </div>

    <!-- Opção de ordenação -->
    <div class="form-group mt-4">
      <label for="sort">Ordenar</label>
      <select id="sort" v-model="sortOrder" @change="sortProducts" class="form-control">
        <option value="default">Selecione o Filtro</option>
        <option value="asc">Preço Crescente</option>
        <option value="desc">Preço Decrescente</option>
      </select>
    </div>

    <!-- Tabela de produtos -->
    <table class="table mt-4">
      <thead>
        <tr>
          <th>Id</th>
          <th>Produto</th>
          <th>Preço</th>
          <th>Código de Barras</th>
          <th>Imagem</th>
          <th colspan="2"></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="product in filteredProducts" :key="product.id">
          <td>{{ product.id }}</td>
          <td>{{ product.title }}</td>
          <td>{{ product.price }}</td>
          <td>{{ product.barcode }}</td>
          <td><img :src="product.image" alt="Product Image" width="100"></td>
          <td><button class="btn btn-primary" @click="viewProduct(product)">Editar</button></td>
          <td><button class="btn btn-danger" @click="deleteProduct(product.id)">Excluir</button></td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  props: ['msg'],
  data() {
    return {
      products: [],
      filteredProducts: [],
      currentProduct: {
        id: null,
        title: '',
        price: 0,
        barcode: '',
        image: ''
      },
      isEditing: false,
      searchQuery: '',
      sortOrder: 'default',
      hasAttemptedToSendProducts: false // Nova variável para controlar o envio
    };
  },
  mounted() {
    this.fetchProducts();
  },
  methods: {
    async fetchProducts() {
      try {
        const response = await axios.get('https://fakestoreapi.com/products');
        this.products = response.data;
        this.filterProducts();
        
        // Envia os produtos para o back-end apenas uma vez
        if (!this.hasAttemptedToSendProducts) {
          await this.sendProductsToBackend(this.products);
          this.hasAttemptedToSendProducts = true;
        }
        
      } catch (error) {
        console.error('Error fetching products:', error);
      }
    },
    filterProducts() {
      if (!this.searchQuery) {
        this.filteredProducts = this.products;
      } else {
        const query = this.searchQuery.toLowerCase();
        this.filteredProducts = this.products.filter(product =>
          (product.title && product.title.toLowerCase().includes(query)) ||
          (product.barcode && product.barcode.includes(query))
        );
      }
      this.sortProducts(); // Aplica a ordenação após o filtro
    },
    sortProducts() {
      if (this.sortOrder === 'default') {
        this.filteredProducts = [...this.filteredProducts].sort((a, b) => a.id - b.id);
      } else if (this.sortOrder === 'asc') {
        this.filteredProducts = [...this.filteredProducts].sort((a, b) => a.price - b.price);
      } else if (this.sortOrder === 'desc') {
        this.filteredProducts = [...this.filteredProducts].sort((a, b) => b.price - a.price);
      }
    },
    async sendProductsToBackend(products) {
      try {
        // Chama o endpoint da sua API .NET Core para inserir os produtos
        await axios.post('http://localhost:5000/api/products/batchInsert', products);
        alert('Produtos inseridos no banco de dados com sucesso!');
      } catch (error) {
        console.error('Erro ao enviar produtos para o backend:', error);
        alert('Erro ao enviar produtos para o backend.');
      }
    },
    async addProduct() {
      try {
        const response = await axios.post('https://fakestoreapi.com/products', this.currentProduct);
        this.products.push(response.data);
        this.filterProducts(); // Atualiza a lista filtrada e ordenada
        this.resetForm();
        alert('Produto adicionado com sucesso!');
      } catch (error) {
        console.error('Error adding product:', error);
        alert('Erro ao adicionar o produto.');
      }
    },
    async viewProduct(product) {
      this.currentProduct = { ...product }; 
      this.isEditing = true; 
    },
    async updateProduct() {
      try {
        const response = await axios.put(`https://fakestoreapi.com/products/${this.currentProduct.id}`, this.currentProduct);
        const index = this.products.findIndex(product => product.id === this.currentProduct.id);
        if (index !== -1) {
          this.products[index] = response.data;
        }
        this.filterProducts();
        this.resetForm();
        alert('Produto atualizado com sucesso!');
      } catch (error) {
        console.error('Error updating product:', error);
        alert('Erro ao atualizar o produto.');
      }
    },
    async deleteProduct(id) {
      const isConfirmed = confirm('Tem certeza de que deseja excluir este produto?');
      if (isConfirmed) {
        try {
          await axios.delete(`https://fakestoreapi.com/products/${id}`);
          this.products = this.products.filter(product => product.id !== id);
          this.filterProducts(); 
          alert('Produto excluído com sucesso!');
        } catch (error) {
          console.error('Error deleting product:', error);
          alert('Erro ao excluir o produto.');
        }
      } else {
        alert('A exclusão do produto foi cancelada.');
      }
    },
    resetForm() {
      this.currentProduct = {
        id: null,
        title: '',
        price: 0,
        barcode: '',
        image: ''
      };
      this.isEditing = false; 
    }
  },
  watch: {
    searchQuery() {
      this.filterProducts();
    },
    sortOrder() {
      this.sortProducts(); 
    }
  }
}
</script>
