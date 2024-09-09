<template>
  <div class="container" id="app">
    <div class="nav-scroller py-1 mb-2">
      <nav class="nav d-flex justify-content-between">
        <!-- Use <router-link> para navegação -->
        <router-link class="p-2 text-muted" to="/">Home</router-link>
        <router-link class="p-2 text-muted" to="/ProductList">Produtos</router-link>
        <router-link class="p-2 text-muted" to="/AboutPage">Sobre</router-link>
      </nav> 
      <ProductList msg="Produtos da Fake Store Api" />
    </div>
  
    <!-- Renderiza o componente da rota atual -->
    <router-view />
  </div>
</template>

<script>
export default {
  name: 'App',
};
</script>
