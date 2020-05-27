﻿using Skybrud.Integrations.BorgerDk.WebService;

namespace Skybrud.Integrations.BorgerDk {

    public class BorgerDkArticleShortDescription {

        public int Id { get; }

        public string Title { get; }

        public BorgerDkArticleShortDescription(ArticleShortDescription description) {
            Id = description.ArticleID;
            Title = description.ArticleTitle;
        }

    }

}