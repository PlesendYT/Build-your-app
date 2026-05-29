"""Blog views: post listing, detail, and comment creation."""

from django.views.generic import ListView, DetailView
from django.views.generic.edit import CreateView
from .models import Post, Comment


class PostListView(ListView):
    """Paginated list of blog posts."""

    model = Post
    template_name = 'post_list.html'
    context_object_name = 'posts'
    paginate_by = 10


class PostDetailView(DetailView):
    """Detail view for a single post with its comments."""

    model = Post
    template_name = 'post_detail.html'

    def get_context_data(self, **kwargs):
        """Add the post's comments to the template context."""
        context = super().get_context_data(**kwargs)
        context['comments'] = self.object.comments.all()
        return context


class CommentCreateView(CreateView):
    """Form view to create a comment on a post."""

    model = Comment
    fields = ['author', 'text']
    template_name = 'comment_form.html'

    def form_valid(self, form):
        """Assign the comment to the correct post before saving."""
        form.instance.post_id = self.kwargs['pk']
        return super().form_valid(form)
