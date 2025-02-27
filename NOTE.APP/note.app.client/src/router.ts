import { createRouter, createWebHistory } from 'vue-router';
import NotesList from '@/views/NoteList.vue';
import NoteForm from '@/views/NoteForm.vue';
import NotFound from '@/views/NotFound.vue';
import EditNote from '@/views/EditNote.vue';

const routes = [
  { path: '/', name: "NoteList", component: NotesList }, 
  { path: '/:pathMatch(.*)*', component: NotFound } 
  ,
  {
    path: '/notes/edit/:userId/:id',
    name: 'EditNote',
    component: EditNote,
    props: true,
  },
  {
    path: '/notes/Detail/:userId/:id',
    name: 'NoteForm',
    component: NoteForm,
    props: true,
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
