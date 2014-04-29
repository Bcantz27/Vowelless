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

#include "graphics/dgl.h"
#include "console/consoleTypes.h"
#include "2d/core/Utility.h"
// Script bindings.

#ifndef _Scene_Object_3D_H_
#include "Scene_Object_3D.h"
#endif

#include "Scene_Object_3D_ScriptBinding.h"

//-----------------------------------------------------------------------------

IMPLEMENT_CONOBJECT( Scene_Object_3D );

//----------------------------------------------------------------------------

Scene_Object_3D::Scene_Object_3D() :
    mLineColor(ColorF(1.0f,1.0f,1.0f,1.0f)),
    mFillColor(ColorF(0.5f,0.5f,0.5f,1.0f)),
    mFillMode(false),
    mPolygonScale( 1.0f, 1.0f )
{
    // Set Vector Associations.
    VECTOR_SET_ASSOCIATION( mPolygonBasisList );
    VECTOR_SET_ASSOCIATION( mPolygonLocalList );

   // Use a static body by default.
   mBodyDefinition.type = b2_dynamicBody;

   mVector_3D_Rotation[0]=0;
   mVector_3D_Rotation[1]=0;
   mVector_3D_Rotation[2]=0;

}

//----------------------------------------------------------------------------

Scene_Object_3D::~Scene_Object_3D()
{
}

//----------------------------------------------------------------------------

void Scene_Object_3D::initPersistFields()
{
   addProtectedField("PolyList", TypePoint2FVector, Offset(mPolygonBasisList, Scene_Object_3D), &setPolyList, &defaultProtectedGetFn, &writePolyList, "");
   addField("LineColor", TypeColorF, Offset(mLineColor, Scene_Object_3D), &writeLineColor, "");
   addField("FillColor", TypeColorF, Offset(mFillColor, Scene_Object_3D), &writeFillColor, "");
   addField("FillMode", TypeBool, Offset(mFillMode, Scene_Object_3D), &writeFillMode, "");

   Parent::initPersistFields();
}

//----------------------------------------------------------------------------

void Scene_Object_3D::copyTo(SimObject* obj)
{
   Parent::copyTo(obj);

   AssertFatal(dynamic_cast<Scene_Object_3D*>(obj), "Scene_Object_3D::copyTo() - Object is not the correct type.");
   Scene_Object_3D* object = static_cast<Scene_Object_3D*>(obj);

   // Copy fields
   object->mFillMode = mFillMode;
   object->mFillColor = mFillColor;
   object->mLineColor = mLineColor;

   if (getPolyVertexCount() > 0)
       object->setPolyCustom(mPolygonBasisList.size(), getPoly());
}

//----------------------------------------------------------------------------

bool Scene_Object_3D::onAdd()
{
   // Call Parent.
   if(!Parent::onAdd())
      return false;

   // Return Okay.
   return true;
}

//----------------------------------------------------------------------------

void Scene_Object_3D::onRemove()
{
   // Call Parent.
   Parent::onRemove();
}

//----------------------------------------------------------------------------

void Scene_Object_3D::sceneRender( const SceneRenderState* pSceneRenderState, const SceneRenderRequest* pSceneRenderRequest, BatchRender* pBatchRenderer )
{
    // Fetch Vertex Count.
    const U32 vertexCount = mPolygonLocalList.size();

    // Finish if not vertices.
    if ( vertexCount == 0 )
        return;

    // Disable Texturing.
    glDisable( GL_TEXTURE_2D );

    glMatrixMode(GL_PROJECTION);
    glPushMatrix();

    glOrtho(-1.0,1.0,-1.0,1.0,1.0,20.0);

    // Save Model-view.
    glMatrixMode(GL_MODELVIEW);
    glPushMatrix();

    // Fetch Position/Rotation.
    const Vector2 position = getRenderPosition();

    // Set Blend Options.
    setBlendOptions();

        // Move into Vector-Space.
        glTranslatef( position.x, position.y, 0.0f );
        glRotatef( mRadToDeg(getRenderAngle()), 0.0f, 0.0f, 1.0f );
        renderPolygonShape(vertexCount);

    // Restore color.
    glColor4f( 1,1,1,1 );

    // Restore Matrix.
    glPopMatrix();
    glPopMatrix();
}

void Scene_Object_3D::renderPolygonShape(U32 vertexCount)
{
#if defined(TORQUE_OS_IOS) || defined(TORQUE_OS_ANDROID)
    // Fill Mode?
    if ( mFillMode )
    {
        // Yes, so set polygon mode to FILL.
        //glPolygonMode( GL_FRONT_AND_BACK, GL_FILL );

        // Set Fill color.
        glColor4f( (GLfloat)mFillColor.red, (GLfloat)mFillColor.green, (GLfloat)mFillColor.blue, (GLfloat)mFillColor.alpha );

        GLfloat vert1[] = {//get first vert and make triangles based off of this one
            (GLfloat)(mPolygonLocalList[0].x),
            (GLfloat)(mPolygonLocalList[0].y),
        };
        GLfloat prevVert[] = {
            (GLfloat)(mPolygonLocalList[1].x),
            (GLfloat)(mPolygonLocalList[1].y),
        };


        // Draw Object.
            for ( U32 n = 2; n < vertexCount; n++ )
            {
                //glVertex2fv ( (GLfloat*)&(mPolygonLocalList[n]) );
                GLfloat vertex[] = {
                                    vert1[0], vert1[1],
                                    (GLfloat)(mPolygonLocalList[n].x), (GLfloat)(mPolygonLocalList[n].y),
                                    prevVert[0], prevVert[1],
                };

                glVertexPointer(2, GL_FLOAT, 0, vertex );
                glDrawArrays(GL_TRIANGLES, 0, 3 );
                prevVert[0] = (GLfloat)(mPolygonLocalList[n].x);//save the current one's for nxt time
                prevVert[1] = (GLfloat)(mPolygonLocalList[n].y);
            }
        //glDrawArrays(GL_TRIANGLES, 0, vertexCount);
        //glEnd();

    }

    // Set Line color.
    glColor4f(mLineColor.red, mLineColor.green, mLineColor.blue, mLineColor.alpha );

        for ( U32 n = 1; n <= vertexCount; n++ )
        {
            GLfloat verts[] = {
                (GLfloat)(mPolygonLocalList[n - 1].x),
                (GLfloat)(mPolygonLocalList[n - 1].y),
                (GLfloat)(mPolygonLocalList[n == vertexCount ? 0 : n].x),
                (GLfloat)(mPolygonLocalList[n == vertexCount ? 0 : n].y),
            };

            glVertexPointer(2, GL_FLOAT, 0, verts );
            glDrawArrays(GL_LINE_LOOP, 0, 2);//draw last two
        }

#else
    // Fill Mode?
    if ( mFillMode )
    {
        // Yes, so set polygon mode to FILL.
        glPolygonMode( GL_FRONT_AND_BACK, GL_FILL );

        // Set Fill color.
        glColor4fv( (GLfloat*)&mFillColor );

        F32 Vector_3D_Vertices[3];

        glRotatef(mVector_3D_Rotation[0],1,0,0);
        glRotatef(mVector_3D_Rotation[1],0,1,0);
        glRotatef(mVector_3D_Rotation[2],0,0,1);

        glBegin( GL_POLYGON );
            for ( U32 n = 0; n < vertexCount; n++ )
            {
                glColor3f(0.0,0.5,0.5);
                Vector_3D_Vertices[0]=mPolygonLocalList[n].x;
                Vector_3D_Vertices[1]=mPolygonLocalList[n].y;
                Vector_3D_Vertices[2]=mPolygonZList[n].x;
                glVertex3fv ( Vector_3D_Vertices );
            }
        glEnd();
    }

#endif
}

//----------------------------------------------------------------------------

void Scene_Object_3D::setSize( const Vector2& size )
{
    F32 xDifference = mSize.x / size.x;

    // Call Parent.
    Parent::setSize( size );

        // Generate Local Polygon.
        generateLocalPoly();

}

//----------------------------------------------------------------------------

void Scene_Object_3D::setPolyScale( const Vector2& scale )
{
    // Check Scales.
    if ( scale.x <= 0.0f || scale.y <= 0.0f )
    {
        Con::warnf("Scene_Object_3D::setPolyScale() - Polygon Scales must be greater than zero! '%g,%g'.", scale.x, scale.y);
        return;
    }
    // Check Scales.
    if ( scale.x > 1.0f || scale.y > 1.0f )
    {
        Con::warnf("Scene_Object_3D::setPolyScale() - Polygon Scales cannot be greater than one! '%g,%g'.", scale.x, scale.y);
        return;
    }

    // Set Polygon Scale.
    mPolygonScale = scale;

    // Generation Local Poly.
    generateLocalPoly();
}

//----------------------------------------------------------------------------

void Scene_Object_3D::setPolyPrimitive( const U32 polyVertexCount )
{
    // Check it's not zero!
    if ( polyVertexCount == 0 )
    {
        // Warn.
        Con::warnf("Scene_Object_3D::setPolyPrimitive() - Vertex count must be greater than zero!");
        // Finish Here.
        return;
    }

    // Clear Polygon List.
    mPolygonBasisList.clear();
    mPolygonBasisList.setSize( polyVertexCount );

    // Point?
    if ( polyVertexCount == 1 )
    {
        // Set Polygon Point.
        mPolygonBasisList[0].Set(0.0f, 0.0f);
    }
    // Special-Case Quad?
    else if ( polyVertexCount == 4 )
    {
        // Yes, so set Quad.
        mPolygonBasisList[0].Set(-0.5f, -0.5f);
        mPolygonBasisList[1].Set(+0.5f, -0.5f);
        mPolygonBasisList[2].Set(+0.5f, +0.5f);
        mPolygonBasisList[3].Set(-0.5f, +0.5f);
    }
    else
    {
        // No, so calculate Regular (Primitive) Polygon Stepping.
        //
        // NOTE:-   The polygon sits on an circle that subscribes the interior
        //          of the collision box.
        F32 angle = M_PI_F / polyVertexCount;
        const F32 angleStep = M_2PI_F / polyVertexCount;

        // Calculate Polygon.
        for ( U32 n = 0; n < polyVertexCount; n++ )
        {
            // Calculate Angle.
            angle += angleStep;
            // Store Polygon Vertex.
            mPolygonBasisList[n].Set(mCos(angle), mSin(angle));
        }
    }

    // Generation Local Poly.
    generateLocalPoly();
}

//----------------------------------------------------------------------------

void Scene_Object_3D::setPolyPrimitiveZ( const U32 polyVertexCount, const char* pCustomPolygon )
{
    // Validate Polygon.
    if ( polyVertexCount < 1 )
    {
        // Warn.
        Con::warnf("Scene_Object_3D::setPolyCustom() - Vertex count must be greater than zero!");
        return;
    }

    // Fetch Custom Polygon Value Count.
    const U32 customCount = Utility::mGetStringElementCount(pCustomPolygon);

    // Validate Polygon Custom Length.
    if ( customCount != polyVertexCount*2 )
    {
        // Warn.
        Con::warnf("Scene_Object_3D::setPolyCustom() - Invalid Custom Polygon Items '%d'; expected '%d'!", customCount, polyVertexCount*2 );
        return;
    }

    //// Validate Polygon Vertices.
    //for ( U32 n = 0; n < customCount; n+=2 )
    //{
    //    // Fetch Coordinate.
    //    const Vector2 coord = Utility::mGetStringElementVector(pCustomPolygon, n);
    //    // Check Range.
    //    if ( coord.x < -1.0f || coord.x > 1.0f || coord.y < -1.0f || coord.y > 1.0f )
    //    {
    //        // Warn.
    //        Con::warnf("Scene_Object_3D::setPolyCustom() - Invalid Polygon Coordinate range; Must be -1 to +1! '(%g,%g)'", coord.x, coord.y );
    //        return;
    //    }
    //}

    // Clear Polygon Basis List.
    mPolygonZList.clear();
    mPolygonZList.setSize( polyVertexCount );

    // Validate Polygon Vertices.
    for ( U32 n = 0; n < polyVertexCount; n++ )
    {
        // Fetch Coordinate.
        const F32 x = dAtof(Utility::mGetStringElement(pCustomPolygon, n*2));
        const F32 y = dAtof(Utility::mGetStringElement(pCustomPolygon, n*2+1));

        // Store Polygon Vertex.
        mPolygonZList[n].Set(x, y);
    }

}

//----------------------------------------------------------------------------

void Scene_Object_3D::setPolyPrimitiveRotation( const char* pRotation )
{

	mVector_3D_Rotation[0] = dAtof(Utility::mGetStringElement(pRotation, 0));
	mVector_3D_Rotation[1] = dAtof(Utility::mGetStringElement(pRotation, 1));
	mVector_3D_Rotation[2] = dAtof(Utility::mGetStringElement(pRotation, 2));

}

//----------------------------------------------------------------------------

const char* Scene_Object_3D::getPolyPrimitiveRotation( void )
{

    // Set Max Buffer Size.
	const U32 maxBufferSize = getPolyVertexCount() * 18 + 1;

    // Get Return Buffer.
    char* pReturnBuffer = Con::getReturnBuffer( maxBufferSize );

    // Check Buffer.
    if( !pReturnBuffer )
    {
        // Warn.
        Con::printf("Scene_Object_3D::getPoly() - Unable to allocate buffer!");
        // Exit.
        return NULL;
    }

    // Set Buffer Counter.
    U32 bufferCount = 0;

    // Add Polygon Edges.
    for ( U32 n = 0; n < 3; n++ )
    {
        // Output Object ID.
        bufferCount += dSprintf( pReturnBuffer + bufferCount, maxBufferSize-bufferCount, "%0.5f ", mVector_3D_Rotation[n] );

        // Finish early if we run out of buffer space.
        if ( bufferCount >= maxBufferSize )
        {
            // Warn.
            Con::warnf("Scene_Object_3D::getPoly() - Error writing to buffer!");
            break;
        }
    }

    // Return Buffer.
    return pReturnBuffer;
}

//----------------------------------------------------------------------------

void Scene_Object_3D::setPolyCustom( const U32 polyVertexCount, const char* pCustomPolygon )
{
    // Validate Polygon.
    if ( polyVertexCount < 1 )
    {
        // Warn.
        Con::warnf("Scene_Object_3D::setPolyCustom() - Vertex count must be greater than zero!");
        return;
    }

    // Fetch Custom Polygon Value Count.
    const U32 customCount = Utility::mGetStringElementCount(pCustomPolygon);

    // Validate Polygon Custom Length.
    if ( customCount != polyVertexCount*2 )
    {
        // Warn.
        Con::warnf("Scene_Object_3D::setPolyCustom() - Invalid Custom Polygon Items '%d'; expected '%d'!", customCount, polyVertexCount*2 );
        return;
    }

    //// Validate Polygon Vertices.
    //for ( U32 n = 0; n < customCount; n+=2 )
    //{
    //    // Fetch Coordinate.
    //    const Vector2 coord = Utility::mGetStringElementVector(pCustomPolygon, n);
    //    // Check Range.
    //    if ( coord.x < -1.0f || coord.x > 1.0f || coord.y < -1.0f || coord.y > 1.0f )
    //    {
    //        // Warn.
    //        Con::warnf("Scene_Object_3D::setPolyCustom() - Invalid Polygon Coordinate range; Must be -1 to +1! '(%g,%g)'", coord.x, coord.y );
    //        return;
    //    }
    //}

    // Clear Polygon Basis List.
    mPolygonBasisList.clear();
    mPolygonBasisList.setSize( polyVertexCount );

    // Validate Polygon Vertices.
    for ( U32 n = 0; n < polyVertexCount; n++ )
    {
        // Fetch Coordinate.
        const F32 x = dAtof(Utility::mGetStringElement(pCustomPolygon, n*2));
        const F32 y = dAtof(Utility::mGetStringElement(pCustomPolygon, n*2+1));

        // Store Polygon Vertex.
        mPolygonBasisList[n].Set(x, y);
    }

    // Generation Local Poly.
    generateLocalPoly();
}

//----------------------------------------------------------------------------

const char* Scene_Object_3D::getPoly( void )
{
    // Get Collision Polygon.
    const Vector2* pPoly = (getPolyVertexCount() > 0) ? getPolyBasis() : NULL;

    // Set Max Buffer Size.
    const U32 maxBufferSize = getPolyVertexCount() * 18 + 1;

    // Get Return Buffer.
    char* pReturnBuffer = Con::getReturnBuffer( maxBufferSize );

    // Check Buffer.
    if( !pReturnBuffer )
    {
        // Warn.
        Con::printf("Scene_Object_3D::getPoly() - Unable to allocate buffer!");
        // Exit.
        return NULL;
    }

    // Set Buffer Counter.
    U32 bufferCount = 0;

    // Add Polygon Edges.
    for ( U32 n = 0; n < getPolyVertexCount(); n++ )
    {
        // Output Object ID.
        bufferCount += dSprintf( pReturnBuffer + bufferCount, maxBufferSize-bufferCount, "%0.5f %0.5f ", pPoly[n].x, pPoly[n].y );

        // Finish early if we run out of buffer space.
        if ( bufferCount >= maxBufferSize )
        {
            // Warn.
            Con::warnf("Scene_Object_3D::getPoly() - Error writing to buffer!");
            break;
        }
    }

    // Return Buffer.
    return pReturnBuffer;
}

//----------------------------------------------------------------------------

const char* Scene_Object_3D::getWorldPoly( void )
{
    // Get the object space polygon
    //const Vector2* pPoly = (getPolyVertexCount() > 0) ? getPolyBasis() : NULL;

    // Set the max buffer size
    const U32 maxBufferSize = getPolyVertexCount() * 18 + 1;

    // Get the return buffer.
    char* pReturnBuffer = Con::getReturnBuffer( maxBufferSize );

    // Check the buffer.
    if( !pReturnBuffer )
    {
        // Warn.
        Con::printf("Scene_Object_3D::getWorldPoly() - Unable to allocate buffer!");

        // Exit.
        return NULL;
    }

    // Set Buffer Counter.
    U32 bufferCount = 0;

    // Add Polygon Edges.
    for ( U32 n = 0; n < getPolyVertexCount(); n++ )
    {
        // Convert the poly point to a world coordinate
        Vector2 worldPoint = getWorldPoint(mPolygonLocalList[n]);

        // Output the point
        bufferCount += dSprintf( pReturnBuffer + bufferCount, maxBufferSize-bufferCount, "%0.5f %0.5f ", worldPoint.x, worldPoint.y );

        // Finish early if we run out of buffer space.
        if ( bufferCount >= maxBufferSize )
        {
            // Warn.
            Con::warnf("Scene_Object_3D::getWorldPoly() - Error writing to buffer!");
            break;
        }
    }

    // Return Buffer.
    return pReturnBuffer;
}

//----------------------------------------------------------------------------

void Scene_Object_3D::generateLocalPoly( void )
{
    // Fetch Polygon Vertex Count.
    const U32 polyVertexCount = mPolygonBasisList.size();

    // Process Collision Polygon (if we've got one).
    if ( polyVertexCount > 0 )
    {
        // Clear Polygon List.
        mPolygonLocalList.clear();
        mPolygonLocalList.setSize( polyVertexCount );

        // Fetch Half Size.
        const Vector2 halfSize = getHalfSize();

        // Calculate Polygon Half-Size.
        const Vector2 polyHalfSize( halfSize.x * mPolygonScale.x, halfSize.y * mPolygonScale.y );

        // Scale/Orientate Polygon.
        for ( U32 n = 0; n < polyVertexCount; n++ )
        {
            // Fetch Polygon Basis.
            Vector2 polyVertex = mPolygonBasisList[n];
            // Scale.
            polyVertex.Set( polyVertex.x * mSize.x * 1/*(mFlipX ? -1.0f : 1.0f)*/,
                            polyVertex.y * mSize.y * 1/*(mFlipY ? -1.0f : 1.0f)*/);
            // Set Vertex.
            mPolygonLocalList[n] = polyVertex;
        }
    }
}

//----------------------------------------------------------------------------

Vector2 Scene_Object_3D::getBoxFromPoints()
{
    Vector2 box(1.0f, 1.0f);

     // Fetch Polygon Vertex Count.
    const U32 polyVertexCount = mPolygonBasisList.size();

    F32 minX = 0;
    F32 minY = 0;
    F32 maxX = 0;
    F32 maxY = 0;

    // Process Collision Polygon (if we've got one).
    if ( polyVertexCount > 0 )
    {
        // Scale/Orientate Polygon.
        for ( U32 n = 0; n < polyVertexCount; n++ )
        {
            // Fetch Polygon Basis.
            Vector2 polyVertex = mPolygonBasisList[n];

            if (polyVertex.x > maxX)
                maxX = polyVertex.x;
            else if (polyVertex.x < minX)
                minX = polyVertex.x;

            if (polyVertex.y > maxY)
                maxY = polyVertex.y;
            else if (polyVertex.y < minY)
                minY = polyVertex.y;
        }
    }

    box.x = maxX - minX;
    box.y = maxY - minY;

    return box;
}
