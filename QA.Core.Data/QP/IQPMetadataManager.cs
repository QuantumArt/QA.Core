using System.Collections.Generic;
using Quantumart.QPublishing.Info;
using Quantumart.QPublishing.Database;

namespace QA.Core.Data.QP
{
    /// <summary>
    /// �������� ��������� ���������� QP
    /// </summary>
    public interface IQPMetadataManager
    {
        /// <summary>
        /// ���������� ������ ��������� ��������
        /// </summary>
        /// <param name="siteName">��� �����</param>
        /// <param name="contentName">��� ��������</param>
        /// <returns></returns>
        List<ContentAttribute> GetContentAttributes(
            string siteName,
            string contentName);

        /// <summary>
        /// ���������� ������ ��������� ��������
        /// </summary>
        /// <param name="contentId">������������� ��������</param>
        /// <returns></returns>
        List<ContentAttribute> GetContentAttributes(
            int contentId);

        /// <summary>
        /// ���������� ������� ��������
        /// </summary>
        /// <param name="siteName">��� �����</param>
        /// <param name="contentName">��� ��������</param>
        /// <param name="fieldName">��� ����</param>
        /// <returns></returns>
        ContentAttribute GetContentAttribute(
            string siteName,
            string contentName,
            string fieldName);

        /// <summary>
        /// ���������� ������������� ��������
        /// </summary>
        /// <param name="siteName">��� �����</param>
        /// <param name="contentName">��� ��������</param>
        /// <returns></returns>
        int GetContentId(
            string siteName,
            string contentName);

        /// <summary>
        /// ���������� ��� ��������
        /// </summary>
        /// <param name="contentId">������������� ��������</param>
        /// <returns></returns>
        string GetContentName(
            int contentId);

        /// <summary>
        /// ���������� ������������� �����
        /// </summary>
        /// <param name="siteName">�������� �����</param>
        /// <returns></returns>
        int GetSiteId(string siteName);

        /// <summary>
        /// ����������� � QP
        /// </summary>
        DBConnector DbConnection { get; }

        /// <summary>
        /// ������ �����������
        /// </summary>
        string ConnectionString { get; }
    }
}
