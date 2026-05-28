from django.views.generic import ListView, DetailView
from django.views.generic.edit import CreateView
from .models import Post, Comment

class PostListView(ListView):
    model = Post
    template_name = 'post_list.html'
    context_object_name = 'posts'
    paginate_by = 10

class PostDetailView(DetailView):
    model = Post
    template_name = 'post_detail.html'

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        context['comments'] = self.object.comments.all()
        return context

class CommentCreateView(CreateView):
    model = Comment
    fields = ['author', 'text']
    template_name = 'comment_form.html'

    def form_valid(self, form):
        form.instance.post_id = self.kwargs['pk']
        return super().form_valid(form)
