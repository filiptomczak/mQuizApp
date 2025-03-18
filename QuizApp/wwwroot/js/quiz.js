function Delete(id) {
    $.ajax({
        url: `/quizzes/delete?id=${id}`,
        type:'DELETE',
    })
}