import { createApp } from 'vue';
import App from './App.vue';
import ProductList from './components/ProductList.vue';
import HomePage from './components/HomePage.vue';
import AboutPage from './components/AboutPage.vue';
import { createRouter, createWebHistory } from 'vue-router';


const routes = [
  { path: "/", component: HomePage },
  { path: "/ProductList", component: ProductList, props: { msg: 'Produtos da Fake Store Api' } },
  { path: "/AboutPage", component: AboutPage },
];


const router = createRouter({
  history: createWebHistory(),
  routes,
});

const app = createApp(App);

app.use(router);

app.mount('#app');
