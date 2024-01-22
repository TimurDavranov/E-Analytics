using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.AI
{
    public struct Constants
    {
        public const string feature = "Features";
        public const string label = "Label";
        public const string predictedLabel = "PredictedLabel";
        public const string score = "Score";
        public const string titleFeaturized = "TitleFeaturized";
        public const string predicateTitleFeaturized = "PredicateTitleFeaturized";
    }

    public struct SqlCommandConstants
    {
        public const string learningSourceSql = "with tableData as (select p.Id as \"ProductId\", trim(replace(t.Title, '\"', '')) as \"Title\", (select string_agg(a.Value, ' ') from (SELECT top ( CAST(RAND() * ((SELECT count(*) FROM STRING_SPLIT(trim(replace(t.Title, '\"', '')), ' ')) - 1 + 1) as int) + 1) RTRIM(LTRIM(value)) as \"Value\" FROM STRING_SPLIT(trim(replace(t.Title, '\"', '')), ' ')) a) as \"PredicateTitle\" from ol.ol_product p join ol.ol_translations t on t.OLProductId = p.Id where Title not like '%\\%' and Title not like '%/%' and Title not like '%!%' and Title is not null and Title != '' and Title not like '#%' and Len(Title) > 20 and Title not like '%[0-9]') select * from (select *, len(tableData.PredicateTitle) * 100 / len(tableData.Title) as \"Percentage\" from tableData) td where td.Percentage > 70";
        public const string countProductItems = "select count(*) from ol.ol_product";
    }
}