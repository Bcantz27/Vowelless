//-----------------------------------------------------------------------------
// Copyright (c) 2013 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

#ifndef _Scene_Object_3D_H_
#define _Scene_Object_3D_H_

#ifndef _SCENE_OBJECT_H_
#include "2d/sceneobject/SceneObject.h"
#endif

//-----------------------------------------------------------------------------

class Scene_Object_3D : public SceneObject
{
private:
    typedef SceneObject Parent;

protected:
    ColorF                  mLineColor;
    ColorF                  mFillColor;
    bool                    mFillMode;
    Vector2                 mPolygonScale;          ///< Polygon Scale.
    Vector<Vector2>         mPolygonBasisList;      ///< Polygon Basis List.
    Vector<Vector2>         mPolygonLocalList;      ///< Polygon Local List.
    Vector<Vector2>         mPolygonZList;          ///< Polygon Z List.
    F32                     mVector_3D_Rotation[3]; ///< Polygon Rotation

public:
    Scene_Object_3D();
    ~Scene_Object_3D();

    /// Reset asset snapshot.
    inline void resetSnapshot() { clearDynamicFields(); }

    static void initPersistFields();

    void setPolyScale( const Vector2& scale );
    void setPolyPrimitive( const U32 polyVertexCount );
    void setPolyCustom( const U32 polyVertexCount, const char* pCustomPolygon );
    void setPolyPrimitiveZ( const U32 polyVertexCount, const char* pCustomPolygon );
    void setPolyPrimitiveRotation( const char* pRotation );
    const char* getPolyPrimitiveRotation( void );
    U32 getPolyVertexCount( void ) { return U32(mPolygonBasisList.size()); };
    inline const Vector2* getPolyBasis( void ) const { return &(mPolygonBasisList[0]); };
    const char* getPoly( void );
    const char* getWorldPoly( void );

    inline void setLineColor( const ColorF& linecolor ) { mLineColor = linecolor; }
    inline const ColorF& getLineColor( void ) const { return mLineColor; }
    inline void setLineAlpha( const F32 alpha ) { mLineColor.alpha = alpha; }
    inline void setFillColor( const ColorF& fillcolor ) { mFillColor = fillcolor; }
    inline const ColorF& getFillColor( void ) const { return mFillColor; }
    inline void setFillAlpha( const F32 alpha ) { mFillColor.alpha = alpha; }
    inline void setFillMode( const bool fillMode ) { mFillMode = fillMode; }
    inline bool getFillMode( void ) const { return mFillMode; }

    Vector2 getBoxFromPoints( void );

    void generateLocalPoly( void );

    void renderPolygonShape(U32 vertexCount);

    virtual void setSize( const Vector2& size );

    virtual bool onAdd();
    virtual void onRemove();
    virtual void sceneRender( const SceneRenderState* pSceneRenderState, const SceneRenderRequest* pSceneRenderRequest, BatchRender* pBatchRenderer );
    virtual bool validRender( void ) const { return true; }
    virtual bool shouldRender( void ) const { return true; }

    virtual bool isBatchRendered( void ) { return false; }

    void copyTo(SimObject* obj);

    /// Declare Console Object.
    DECLARE_CONOBJECT( Scene_Object_3D );

protected:
    static bool setPolyList(void* obj, const char* data)
    {
       const U32 count = Utility::mGetStringElementCount(data) >> 1;
       static_cast<Scene_Object_3D*>(obj)->setPolyCustom(count, data);
       return false;
    }
    static bool writePolyList( void* obj, StringTableEntry pFieldName ) { return static_cast<Scene_Object_3D*>(obj)->mPolygonBasisList.size() > 0; }
    static bool writeLineColor( void* obj, StringTableEntry pFieldName ) { return static_cast<Scene_Object_3D*>(obj)->mLineColor != ColorF(1.0f,1.0f,1.0f,1.0f); }
    static bool writeFillColor( void* obj, StringTableEntry pFieldName ) { return static_cast<Scene_Object_3D*>(obj)->mFillColor != ColorF(0.5f,0.5f,0.5f,1.0f); }
    static bool writeFillMode( void* obj, StringTableEntry pFieldName ) { return static_cast<Scene_Object_3D*>(obj)->mFillMode == true; }

};

#endif // _Scene_Object_3D_H_
